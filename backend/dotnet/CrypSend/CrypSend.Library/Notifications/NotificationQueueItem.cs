using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library.Notifications
{
    public class NotificationQueueItem
    {
        public string Destination { get; set; }

        public string SecretLocation { get; set; }

        public string SecretId { get; set; }

        public NotificationType Type { get; set; }

        public string Payload { get; set; }

    }
}
