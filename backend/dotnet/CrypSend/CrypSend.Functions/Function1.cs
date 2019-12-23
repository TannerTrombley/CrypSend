using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CrypSend.Repository;
using CrypSend.Library;

namespace CrypSend.Functions
{
    public class Function1
    {
        private readonly ILogger<Function1> log;
        private readonly IRepository<SecretPayload> _secretPayloadRepository;
        private readonly IEncryptionEngineFactory _factory;
        private readonly ICrypSendService _crypSendService;
        public Function1(
            ILogger<Function1> logger,
            IRepository<SecretPayload> repo,
            IEncryptionEngineFactory factory,
            ICrypSendService crypSendService)
        {
            log = logger;
            _secretPayloadRepository = repo;
            _factory = factory;
            _crypSendService = crypSendService;
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> RunPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var engine = _factory.GetEncryptionEngine(EncryptionType.None);

            var secret = new SecretPayload()
            {
                RowKey = "Test123",
                PartitionKey = "Test123",
                EncryptedPayload = await engine.EncryptAsync("PLAINTEXTVALUE")
            };

            await _secretPayloadRepository.UpsertDocumentAsync(secret);

            return new OkObjectResult("Great job, Tanner");
        }

        [FunctionName("Function2")]
        public async Task<IActionResult> RunGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var engine = _factory.GetEncryptionEngine(EncryptionType.None);

            var secret = await _secretPayloadRepository.GetDocumentAsync("Test123", "Test123");

            var plaintext = await engine.DecryptAsync(secret);

            return new OkObjectResult($"Great job, Tanner: {plaintext}");
        }

        [FunctionName("storesecret")]
        public async Task<IActionResult> RunStoreAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "secrets")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<StoreSecretRequest>(requestBody);

            var result = await _crypSendService.StoreSecretAsync(data);

            return new OkObjectResult(result);
        }

        [FunctionName("getsecret")]
        public async Task<IActionResult> RunGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "secrets/{id}")] HttpRequest req, string id)
        {
            var result = await _crypSendService.FetchStoredSecretAsync(id);

            return new OkObjectResult(result);
        }
    }
}
