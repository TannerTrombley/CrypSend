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
        public Function1(ILogger<Function1> logger, IRepository<SecretPayload> repo, IEncryptionEngineFactory factory)
        {
            log = logger;
            _secretPayloadRepository = repo;
            _factory = factory;
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
                EncryptedPayload = engine.Encrypt("PLAINTEXTVALUE")
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

            var plaintext = engine.Decrypt(secret.EncryptedPayload, null);

            return new OkObjectResult($"Great job, Tanner: {plaintext}");
        }
    }
}
