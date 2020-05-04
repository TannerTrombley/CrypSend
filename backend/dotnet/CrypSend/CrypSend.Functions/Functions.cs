using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CrypSend.Repository;
using CrypSend.Library;
using System.Collections;
using CrypSend.Library.Settings;
using CrypSend.Library.Notifications;
using SendGrid.Helpers.Mail;

namespace CrypSend.Functions
{
    public class Functions
    {
        private readonly ILogger<Functions> log;
        private readonly IRepository<SecretPayload> _secretPayloadRepository;
        private readonly IEncryptionEngineFactory _factory;
        private readonly ICrypSendService _crypSendService;
        private readonly ISettingsProvider _settings;
        public Functions(
            ILogger<Functions> logger,
            IRepository<SecretPayload> repo,
            IEncryptionEngineFactory factory,
            ICrypSendService crypSendService,
            ISettingsProvider settings)
        {
            log = logger;
            _secretPayloadRepository = repo;
            _factory = factory;
            _crypSendService = crypSendService;
            _settings = settings;
        }

        //[FunctionName("Function1")]
        //public async Task<IActionResult> RunPost(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    var engine = _factory.GetEncryptionEngine(EncryptionType.None);

        //    var secret = new SecretPayload()
        //    {
        //        RowKey = "Test123",
        //        PartitionKey = "Test123",
        //        EncryptedPayload = await engine.EncryptAsync("PLAINTEXTVALUE")
        //    };

        //    await _secretPayloadRepository.UpsertDocumentAsync(secret);

        //    return new OkObjectResult("Great job, Tanner");
        //}

        //[FunctionName("Function2")]
        //public async Task<IActionResult> RunGet(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    var engine = _factory.GetEncryptionEngine(EncryptionType.None);

        //    var secret = await _secretPayloadRepository.GetDocumentAsync("Test123", "Test123");

        //    var plaintext = await engine.DecryptAsync(secret);

        //    return new OkObjectResult($"Great job, Tanner: {plaintext}");
        //}

        [FunctionName("storesecret")]
        public async Task<IActionResult> RunStoreAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "secrets")] HttpRequest req,
            [Queue("emailnotificationqueue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> emailNotificationQueue,
            [Queue("smsnotificationqueue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> smsNotificationQueue)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<StoreSecretRequest>(requestBody);

            var result = await _crypSendService.StoreSecretAsync(data, emailNotificationQueue);

            //var host = _settings.GetSetting("WEBSITE_HOSTNAME");
            //result.SecretLocation = host + "/" + result.SecretId;

            //if (data.NotificationDestination != null)
            //{
            //    var item = new NotificationQueueItem()
            //    {
            //        Destination = data.NotificationDestination,
            //        SecretLocation = result.SecretLocation,
            //        SecretId = result.SecretId
            //    };

            //    switch (data.NotificationLocation.Type)
            //    {
            //        case NotificationType.Email:
            //            emailNotificationQueue.Add(JsonConvert.SerializeObject(item));
            //            break;
            //        case NotificationType.SMS:
            //            smsNotificationQueue.Add(JsonConvert.SerializeObject(item));
            //            break;
            //        default:
            //            return new BadRequestObjectResult("Unknown NotificationLocation type");
            //    }
            //}

            return new OkObjectResult(result);
        }

        [FunctionName("getsecret")]
        public async Task<IActionResult> RunGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "secrets/{id}")] HttpRequest req,
            [Queue("emailotpqueue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> emailOtpQueue,
            [Queue("smsotpqueue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> smsOtpQueue,
            string id)
        {
            string suppliedOTP = req.Query["otp"];
            var fetchRequest = new FetchStoredSecretRequest()
            {
                SecretId = id,
                OneTimePass = suppliedOTP
            };
            var result = await _crypSendService.FetchStoredSecretAsync(fetchRequest);

            // emailOtpQueue.Add(result.PlainText);
            // smsOtpQueue.Add(result.PlainText);
            return new OkObjectResult(result);
        }

        [FunctionName("emailnotification")]
        public async void RunEmailNotificationAsync(
            [QueueTrigger("emailnotificationqueue", Connection = "AzureWebJobsStorage")]NotificationQueueItem item,
            [SendGrid(ApiKey = "CrypSendApiKey")] IAsyncCollector<SendGridMessage> messageCollector)
        {
            var message = new SendGridMessage();
            message.AddTo(item.Destination);
            message.AddContent("text/html", $"<h1>A secret was shared with you</h1> <br/>Follow the link below and then enter the code your pal shared (or will soon share) with you to unlock the secret <br/> {item.SecretLocation} ");
            message.SetFrom(new EmailAddress("crypsend.notifications@do-not-reply.example.com"));
            message.SetSubject("A Secret Was Shared With You");

            await messageCollector.AddAsync(message);
        }

        //[FunctionName("smsnotification")]
        //[return: TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "+1425XXXXXXX")]
        //public async void RunSmsNotificationAsync(
        //    [QueueTrigger("smsnotificationqueue", Connection = "AzureWebJobsStorage")]NotificationQueueItem item)
        //{
        //    throw new NotImplementedException();
        //}

        //[FunctionName("emailotp")]
        //public async void RunEmailOTPAsync(
        //    [QueueTrigger("emailotpqueue", Connection = "AzureWebJobsStorage")]NotificationQueueItem item,
        //    [SendGrid(ApiKey = "CrypSendApiKey")] IAsyncCollector<SendGridMessage> messageCollector)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
