using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrypSend.Library.Notifications
{
    public class EmailNotificationService : INotificationService
    {
        public EmailNotificationService()
        {

        }

        public Task SendAsync(string recipient, string message)
        {
            throw new NotImplementedException();
        }
    }
}
