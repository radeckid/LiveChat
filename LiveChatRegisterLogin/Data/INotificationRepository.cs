using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public interface INotificationRepository
    {
        Task<Notification> AddInvitation(int requesterId, int newFriendId);
        Task Process(int notificationid, bool isNotificationAccepted);
        Task<bool> UserExists(int userId);
    }
}
