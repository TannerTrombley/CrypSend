namespace CrypSend.Library.Notifications
{
    public interface INotificationServiceFactory
    {
        INotificationService GetNotificationService(NotificationType type);
    }
}
