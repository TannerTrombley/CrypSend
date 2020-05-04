using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library
{
    public class FetchStoredSecretRequest
    {
        public string SecretId { get; set; }

        public string OneTimePass { get; set; }
    }
}
