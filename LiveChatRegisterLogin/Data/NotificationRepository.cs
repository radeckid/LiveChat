using LiveChatRegisterLogin.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using LiveChatRegisterLogin.HubConfig;
using System.Security.Cryptography.X509Certificates;
using System.Data.Entity.Validation;
using LiveChatRegisterLogin.Types;

namespace LiveChatRegisterLogin.Data
{
    public class NotificationRepository : INotificationRepository
    {
        DataContext _context;
        private IHubContext<NotificationHub> _hub;

        public NotificationRepository(DataContext context, IHubContext<NotificationHub> hub)
        {
            if (context != null)
            {
                _context = context;
                _context.Database.EnsureCreated();
            }

            _hub = hub;
        }

        public async Task<IEnumerable<Notification>> GetAllSentNotification(int id)
        {
            return await _context.Notifications.Where(x => x.SenderId.Equals(id)).ToArrayAsync().ConfigureAwait(true);
        }

        public async Task<IEnumerable<Notification>> GetAllReceivedNotification(int id)
        {
            return await _context.Notifications.Where(x => x.ReceiverId.Equals(id)).ToArrayAsync().ConfigureAwait(true);
        }

        public async Task<bool> AddInvitation(int requesterId, int newFriendId)
        {
            var sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == requesterId).ConfigureAwait(true);
            var receiver = await _context.Users.FirstOrDefaultAsync(x => x.Id == newFriendId).ConfigureAwait(true);

            if (sender.Friends.Any(x => x.FriendId == newFriendId))
            {
                return false;
            }

            var invitation = new Notification
            {
                Sender = sender,
                Receiver = receiver,
                Desc = string.Concat("User ", sender.Email, " invited you to friends.\nDo you know him/her ?"),
                Type = Types.NotificationType.Invitation,
                ExtraData = "",
                Date = DateTime.Now
            };

            await _context.Notifications.AddAsync(invitation);
            await _context.SaveChangesAsync().ConfigureAwait(true);

            _ = SendData(invitation);

            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public async Task Process(int notificationid, bool isNotificationAccepted, int userId)
        {
            Notification notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == notificationid).ConfigureAwait(true);
            Notification informationNotification = null;
            if(notification == null)
            {
                throw new ArgumentException("Cannot find any notification with that id");
            }

            var notificationType = notification.Type;

            if(notification.Receiver.Id != userId)
            {
                throw new ArgumentException("This is not your notification");
            }

            switch (notificationType)
            {
                case Types.NotificationType.Invitation:
                    if (isNotificationAccepted)
                    {
                        await AddFriend(notification).ConfigureAwait(true);
                        informationNotification = new Notification
                        {
                            Sender = notification.Receiver,
                            Receiver = notification.Sender,
                            Type = Types.NotificationType.Information,
                            Desc = $"User {notification.Receiver.Email} accepted your invitation.",
                            ExtraData = string.Empty,
                            Date = DateTime.Now
                        };
                    }
                    break;
                case Types.NotificationType.RelationDeletion:
                    break;
            }

            _context.Notifications.Remove(notification);
            if(informationNotification != null)
            {
                await _context.Notifications.AddAsync(informationNotification).ConfigureAwait(true);
            }

            await _context.SaveChangesAsync().ConfigureAwait(true);

            if (informationNotification != null)
            {
                await SendData(informationNotification).ConfigureAwait(true);
            } 
        }

        public async Task<bool> UserExists(int userId)
        {
            if (await _context.Users.AnyAsync(x => x.Id == userId).ConfigureAwait(true))
                return true;

            return false;
        }

        private async Task AddFriend(Notification notification)
        {
            var user1 = notification.Sender;
            var user2 = notification.Receiver;
            Chat chat;
            ChatMembership chatMembershipUser1;
            ChatMembership chatMembershipUser2;

            user1.Friends.Add(new Relation
            {
                FriendId = user2.Id
            });
            user2.Friends.Add(new Relation
            {
                FriendId = user1.Id
            });

            chat = new Chat
            {
                OwnerId = user1.Id,
                Type = Types.ChatType.FriendsChat
            };

            chatMembershipUser1 = new ChatMembership
            {
                Chat = chat,
                User = user1
            };

            chatMembershipUser2 = new ChatMembership
            {
                Chat = chat,
                User = user2
            };

            await _context.ChatMemberships.AddAsync(chatMembershipUser1).ConfigureAwait(true);
            await _context.ChatMemberships.AddAsync(chatMembershipUser2).ConfigureAwait(true);
        }

        public async Task<bool> DeleteRelation(int senderId, int chatId, string reason)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id == chatId).ConfigureAwait(true);

            if (chat == null || (chat.Type != ChatType.FriendsChat && chat.OwnerId != senderId))
            {
                return false;
            }

            var idUsers = await _context.ChatMemberships.Where(x => x.ChatId == chatId).Select(y => y.UserId).ToArrayAsync().ConfigureAwait(true);

            var relations = await _context.Friends.Where(x => idUsers.Contains(x.UserId)).ToArrayAsync().ConfigureAwait(true);

            var senderChatMembership = _context.ChatMemberships.Where(x => x.ChatId == chatId);

            if(relations == null || senderChatMembership == null)
            {
                return false;
            }

            var sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == senderId).ConfigureAwait(true);
            idUsers = idUsers.Where(x => x != senderId).ToArray();
            var receivers = _context.Users.Where(x => idUsers.Contains(x.Id)).ToArray();

            _context.Friends.RemoveRange(relations);
            _context.ChatMemberships.RemoveRange(senderChatMembership);

            IList<Notification> notifications = new List<Notification>();
            foreach(var user in receivers)
            {
                notifications.Add(new Notification
                {
                    Receiver = user,
                    Sender = sender,
                    Type = Types.NotificationType.RelationDeletion,
                    Desc = reason,
                    ExtraData = string.Empty,
                    Date = DateTime.Now
                });
            }

            _context.Notifications.AddRange(notifications);

            await _context.SaveChangesAsync().ConfigureAwait(true);

            if(notifications != null)
            {
                foreach (var notification in notifications)
                {
                    _ = SendData(notification);
                }
            }
            
            return true;
        }

        private async Task SendData(Notification notification)
        {
            if(notification != null)
            {
                object[] param = { new NotificationChartModel
                {
                    Id = notification.Id,
                    SenderEmail = notification.Sender.Email,
                    ReceiverId = notification.Receiver.Id,
                    Type = notification.Type,
                    Desc = notification.Desc,
                    ExtraData = notification.ExtraData,
                    Date = notification.Date
                } };

                await _hub.Clients.All.SendAsync("transfernotifications", param).ConfigureAwait(true);
            }
            
        }
    }
}
