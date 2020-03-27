using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public class NotificationRepository : INotificationRepository
    {
        DataContext _context;

        public NotificationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Notification> AddInvitation(int requesterId, int newFriendId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == requesterId).ConfigureAwait(true);
            var receiver = await _context.Users.FirstOrDefaultAsync(x => x.Id == newFriendId).ConfigureAwait(true);

            if (user.Friends.Any(x => x.FriendId == newFriendId))
            {
                return null;
            }

            var invitation = new Notification
            {
                Sender = user,
                Receiver = receiver,
                Desc = string.Concat("User ", receiver.Email, " invited you to friends.\nDo you know him/her ?"),
                Type = Types.NotificationType.Invitation,
                ExtraData = "",
                Date = DateTime.Now
            };

            await _context.Notifications.AddAsync(invitation);
            await _context.SaveChangesAsync().ConfigureAwait(true);

            return invitation;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public async Task Process(NotificationDTO notificationDTO)
        {
            Notification notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == notificationDTO.Id).ConfigureAwait(true);
            var notificationType = notification.Type;

            switch(notificationType)
            {
                case Types.NotificationType.Invitation:
                    if(notificationDTO.Action)
                    {
                        AddFriend(notification);
                    }
                    _context.Notifications.Remove(notification);
                    break;
            }

            await _context.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task<bool> UserExists(int userId)
        {
            if (await _context.Users.AnyAsync(x => x.Id == userId).ConfigureAwait(true))
                return true;

            return false;
        }

        private void AddFriend(Notification notification)
        {
            var user1 = notification.Sender;
            var user2 = notification.Receiver;

            user1.Friends.Add(new Relation 
            { 
                FriendId = user2.Id
            });
            user2.Friends.Add(new Relation
            {
                FriendId = user1.Id
            });
        }
    }
}
