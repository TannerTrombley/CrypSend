using System.Threading.Tasks;

namespace CrypSend.Library.Notifications
{
    public interface INotificationService
    {
        Task SendAsync(string recipient, string message);
    }
}
