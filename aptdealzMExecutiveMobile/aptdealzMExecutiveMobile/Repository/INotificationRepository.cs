using System.Threading.Tasks;

namespace aptdealzMExecutiveMobile.Repository
{
    public interface INotificationRepository
    {
        Task<string> GetNotificationCount();

        Task<bool> SetUserNoficiationAsRead(string NotificationId);
        Task<bool> SetUserNoficiationAsReadAndDelete(string NotificationId);
    }
}
