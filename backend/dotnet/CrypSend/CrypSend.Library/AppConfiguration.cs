using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library
{
    public class AppConfiguration
    {
        public SecretPayloadRepositorySettings SecretRepositoryConfiguration { get; set; }

        public string AzureWebJobsStorage { get; set; }
    }
}
