using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CrypSend.Functions
{
    public static class emailFunction1
    {
        [FunctionName("emailFunction1")]
        public static void Run([QueueTrigger("myqueue-items", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
