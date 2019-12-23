using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library
{
    public class FetchStoredSecretResponse
    {
        public string PlainText { get; set; }

        public bool RequireVerification { get; set; }
    }
}
