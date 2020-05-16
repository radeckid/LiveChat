using LiveChatRegisterLogin.Containers;
using LiveChatRegisterLogin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public interface INotificationRepository
    {
        Task<bool> AddInvitation(InvitationContainer container);
        Task Process(int notificationid, bool isNotificationAccepted, int userId);
        Task<bool> UserExists(int userId);
        IEnumerable<Notification> GetAllSentNotification(int id);
        IEnumerable<Notification> GetAllReceivedNotification(int id);
        Task<bool> DeleteMembership(DeletionMemberContainer container);
    }
}
