using CrypSend.Library.Notifications;
using CrypSend.Library.OneTimePassword;
using CrypSend.Library.SecretMetadata;
using CrypSend.Library.Settings;
using CrypSend.Repository;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CrypSend.Library
{
    public class CrypSendService : ICrypSendService
    {
        private readonly IEncryptionEngineFactory _encryptionEngineFactory;
        private readonly IRepository<SecretPayload> _secretRepository;
        private readonly IRepository<SecretMetadataDocument> _secretMetadataRepository;
        private readonly IOneTimePasswordGenerator _oneTimePasswordGenerator;
        private readonly ISettingsProvider _settings;

        private const EncryptionType _encryptionType = EncryptionType.OneTimePad;
        private const int _maxAttempts = 3;

        public CrypSendService(
            IEncryptionEngineFactory encryptionEngineFactory,
            IRepository<SecretPayload> secretRepository,
            IRepository<SecretMetadataDocument> secretMetadataRepository,
            IOneTimePasswordGenerator oneTimePasswordGenerator,
            ISettingsProvider settings)
        {
            _encryptionEngineFactory = encryptionEngineFactory;
            _secretRepository = secretRepository;
            _secretMetadataRepository = secretMetadataRepository;
            _oneTimePasswordGenerator = oneTimePasswordGenerator;
            _settings = settings;
        }

        public async Task<FetchStoredSecretResponse> FetchStoredSecretAsync(FetchStoredSecretRequest request)
        {
            var secret = await _secretRepository.GetDocumentAsync(request.SecretId, request.SecretId);

            var secretMetadata = await _secretMetadataRepository.GetDocumentAsync(secret.MetadataId.ToString(), secret.MetadataId.ToString());

            if (!string.IsNullOrWhiteSpace(request.OneTimePass) && secretMetadata.OneTimeCode == request.OneTimePass)
            {
                secret.IsLocked = false;
            }

            // Check for any conditions not met
            if (secret.IsLocked)
            {
                var tooManyAttempts = false;
                secret.Attempts++;
                if (secret.Attempts > _maxAttempts)
                {
                    DeleteSecretAsync(secret);
                    tooManyAttempts = true;
                }
                else
                {
                    await _secretRepository.UpsertDocumentAsync(secret);
                }

                return new FetchStoredSecretResponse()
                {
                    RequireVerification = true,
                    PlainText = null,
                    TooManyAttempts = tooManyAttempts
                };
            }

            var engine = _encryptionEngineFactory.GetEncryptionEngine(secret.EncryptionType);
            var response = new FetchStoredSecretResponse()
            {
                PlainText = await engine.DecryptAsync(secret, secretMetadata),
                RequireVerification = false
            };
            DeleteSecretAsync(secret);
            return response;
        }

        private void DeleteSecretAsync(SecretPayload secret)
        {
            _ = _secretMetadataRepository.DeleteDocumentAsync(secret.MetadataId.ToString(), secret.MetadataId.ToString());
            _ = _secretRepository.DeleteDocumentAsync(secret.RowKey, secret.RowKey);
        }

        public async Task<StoreSecretResponse> StoreSecretAsync(StoreSecretRequest request, ICollector<string> notificationQueue)
        {
            var engine = _encryptionEngineFactory.GetEncryptionEngine(_encryptionType);

            var secretId = Guid.NewGuid();
            var metadataId = Guid.NewGuid();

            var code = _oneTimePasswordGenerator.GenerateOneTimePassword();

            var secretMetadata = new SecretMetadataDocument()
            {
                RowKey = metadataId.ToString(),
                PartitionKey = metadataId.ToString(),
                OneTimeCode = code.Code
            };
            var secret = new SecretPayload()
            {
                RowKey = secretId.ToString(),
                PartitionKey = secretId.ToString(),
                MetadataId = metadataId,
                EncryptionType = _encryptionType,
                EncryptedPayload = await engine.EncryptAsync(request.PlainText, secretMetadata),
                IsLocked = true
            };

            
            await _secretMetadataRepository.UpsertDocumentAsync(secretMetadata);
            await _secretRepository.UpsertDocumentAsync(secret);



            var result = new StoreSecretResponse()
            {
                SecretId = secretId.ToString(),
                OneTimePasscode = code.Code
            };

            var host = _settings.GetRequiredSetting("WebsiteHost");
            result.SecretLocation = host + "/" + result.SecretId;
            var item = new NotificationQueueItem()
            {
                Destination = request.NotificationDestination,
                SecretLocation = result.SecretLocation,
                SecretId = result.SecretId
            };
            notificationQueue.Add(JsonConvert.SerializeObject(item));

            return result;
        }
    }
}
