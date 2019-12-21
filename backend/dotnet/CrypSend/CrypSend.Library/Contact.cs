using CrypSend.Library.Notifications;

namespace CrypSend.Library
{
    public class Contact
    {
        public NotificationType Type { get; set; }

        public bool IsPrimary { get; set; }

        public string Value { get; set; }
    }
}
