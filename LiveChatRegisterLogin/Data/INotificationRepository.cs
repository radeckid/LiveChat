using LiveChatRegisterLogin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public interface INotificationRepository
    {
        Task<bool> AddInvitation(int requesterId, int newFriendId);
        Task Process(int notificationid, bool isNotificationAccepted, int userId);
        Task<bool> UserExists(int userId);
        Task<IEnumerable<Notification>> GetAllSentNotification(int id);
        Task<IEnumerable<Notification>> GetAllReceivedNotification(int id);
        Task<bool> DeleteRelation(int senderId, int receiverId, string reason);
    }
}
