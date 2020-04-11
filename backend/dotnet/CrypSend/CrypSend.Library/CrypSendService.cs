using CrypSend.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CrypSend.Library
{
    public class CrypSendService : ICrypSendService
    {
        private readonly IEncryptionEngineFactory _encryptionEngineFactory;
        private readonly IRepository<SecretPayload> _secretRepository;

        public CrypSendService(
            IEncryptionEngineFactory encryptionEngineFactory,
            IRepository<SecretPayload> secretRepository)
        {
            _encryptionEngineFactory = encryptionEngineFactory;
            _secretRepository = secretRepository;
        }

        public async Task<FetchStoredSecretResponse> FetchStoredSecretAsync(string id)
        {
            var secret = await _secretRepository.GetDocumentAsync(id, id);

            var engine = _encryptionEngineFactory.GetEncryptionEngine(secret.EncryptionType);

            // Check for any conditions not met
            if (secret?.RetrievalConditions?.Any(condition => !condition.HasMetCondition) == true)
            {
                return new FetchStoredSecretResponse()
                {
                    RequireVerification = true,
                    Conditions = secret.RetrievalConditions,
                    PlainText = null
                };
            }

            return new FetchStoredSecretResponse()
            {
                PlainText = await engine.DecryptAsync(secret),
                RequireVerification = false,
                Conditions = null
            };
        }

        public async Task<StoreSecretResponse> StoreSecretAsync(StoreSecretRequest request)
        {
            var engine = _encryptionEngineFactory.GetEncryptionEngine(request.EncryptionType);

            var secretId = Guid.NewGuid();
            var secret = new SecretPayload()
            {
                RowKey = secretId.ToString(),
                PartitionKey = secretId.ToString(),
                EncryptionType = request.EncryptionType,
                EncryptedPayload = await engine.EncryptAsync(request.PlainText),
                Contacts = request.Contacts,
                RetrievalConditions = request.RetrievalConditions,
                NotificationLocation = request.NotificationLocation
            };

            await _secretRepository.UpsertDocumentAsync(secret);

            return new StoreSecretResponse()
            {
                SecretId = secretId.ToString()
            };
        }
    }
}
