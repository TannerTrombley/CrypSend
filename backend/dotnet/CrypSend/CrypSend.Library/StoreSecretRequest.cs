using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library
{
    public class StoreSecretRequest
    {
        public string PlainText { get; set; }

        public string NotificationDestination { get; set; }
    }
}
