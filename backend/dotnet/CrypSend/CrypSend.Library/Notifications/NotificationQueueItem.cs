using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library.Notifications
{
    public class NotificationQueueItem
    {
        public Contact Destination { get; set; }

        public string SecretLocation { get; set; }

        public string SecretId { get; set; }
    }
}
