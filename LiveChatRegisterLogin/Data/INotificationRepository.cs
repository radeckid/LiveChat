using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public interface INotificationRepository
    {
        Task<Notification> AddInvitation(int requesterId, int newFriendId);
        Task Process(NotificationDTO notificationDTO);
        Task<bool> UserExists(int userId);
    }
}
