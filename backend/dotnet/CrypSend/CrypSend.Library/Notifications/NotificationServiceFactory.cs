using System;
using Microsoft.Extensions.DependencyInjection;

namespace CrypSend.Library.Notifications
{
    public class NotificationServiceFactory : INotificationServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public NotificationServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public INotificationService GetNotificationService(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Email:
                    return _serviceProvider.GetRequiredService<EmailNotificationService>();
                default:
                    throw new ArgumentException($"No notification service to handle {type.ToString()}");
            }
        }
    }
}
