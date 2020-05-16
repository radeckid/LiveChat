using LiveChatRegisterLogin.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using LiveChatRegisterLogin.HubConfig;
using LiveChatRegisterLogin.Types;
using LiveChatRegisterLogin.Services;
using LiveChatRegisterLogin.Helpers;
using LiveChatRegisterLogin.Containers;

namespace LiveChatRegisterLogin.Data
{
    public class NotificationRepository : INotificationRepository
    {
        private DataContext _context;
        private IChatRepository _chatRepository;
        private IHubContext<NotificationHub> _hub;
        private IConnectionService _serivce;

        public NotificationRepository(DataContext context, IChatRepository chatRepository, IHubContext<NotificationHub> hub, Func<ServiceTypes, IConnectionService> serviceResolver)
        {
            if (context != null)
            {
                _context = context;
                _context.Database.EnsureCreated();
            }

            _chatRepository = chatRepository;
            _hub = hub;
            _serivce = serviceResolver(ServiceTypes.NotificationConnectionService);
        }

        public IEnumerable<Notification> GetAllSentNotification(int id)
        {
            return _context.Notifications.Where(x => x.SenderId.Equals(id));
        }

        public IEnumerable<Notification> GetAllReceivedNotification(int id)
        {
            return _context.Notifications.Where(x => x.Receiver.Id.Equals(id));
        }

        public async Task<bool> AddInvitation(InvitationContainer container)
        {
            Chat chat = null;
            bool IsFriendInvitation = container.Type.Equals(NotificationType.Invitation);

            if (!IsFriendInvitation)
            {
                chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id.Equals(container.ChatId)).ConfigureAwait(true);

                if(chat == null)
                {
                    return false;
                }

                if (!_context.Friends.Any(x => x.UserId.Equals(container.RequesterId) && x.FriendId.Equals(container.ReceiverId)))
                {
                    return false;
                }
            }

            var sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == container.RequesterId).ConfigureAwait(true);
            var receiver = await _context.Users.FirstOrDefaultAsync(x => x.Id == container.ReceiverId).ConfigureAwait(true);
            
            if(_context.Notifications.Any(x => x.ReceiverId.Equals(container.ReceiverId) && x.SenderId.Equals(container.RequesterId) && x.Type == container.Type))
            {
                return false;
            }

            if (sender.Friends.Any(x => x.FriendId == container.ReceiverId) && chat.Type.Equals(ChatType.FriendsChat))
            {
                return false;
            }

            string notificationDesc;

            if(IsFriendInvitation)
            {
                notificationDesc = string.Concat("User ", sender.Email, " invited you to friends.\nDo you know him/her ?");
            }
            else
            {
                notificationDesc = string.Concat("User ", sender.Email, " invited you to chat ", chat?.Name , ".\nDo you want join to this chat ?");
            }

            var invitation = new Notification
            {
                Sender = sender,
                Receiver = receiver,
                Desc = notificationDesc,
                Type = container.Type,
                ExtraData = IsFriendInvitation ? string.Empty : chat.Id.ToString(),
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

            if(notification == null)
            {
                throw new ArgumentException("Cannot find any notification with that id");
            }

            if (!notification.Receiver.Id.Equals(userId))
            {
                throw new ArgumentException("This is not your notification");
            }

            NotificationProcess process = new NotificationProcess(_chatRepository, notification, isNotificationAccepted);

            await process.Process().ConfigureAwait(true);

            _context.Notifications.Remove(notification);

            if(process.InformationNotification != null)
            {
                await _context.Notifications.AddAsync(process.InformationNotification).ConfigureAwait(true);
            }

            await _context.SaveChangesAsync().ConfigureAwait(true);

            if (process.InformationNotification != null)
            {
                await SendData(process.InformationNotification).ConfigureAwait(true);
            } 
        }

        public async Task<bool> UserExists(int userId)
        {
            if (await _context.Users.AnyAsync(x => x.Id == userId).ConfigureAwait(true))
                return true;

            return false;
        }

        public async Task<bool> DeleteMembership(DeletionMemberContainer container)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id == container.ChatId).ConfigureAwait(true);

            if (chat == null)
            {
                return false;
            }

            bool isChatFriend = chat.Type.Equals(ChatType.FriendsChat);
            bool isOneMember = false;
            var chatMemberships = _context.ChatMemberships.Where(x => x.ChatId == container.ChatId);

            if (chatMemberships == null || !chatMemberships.Any(x => x.UserId.Equals(container.SenderId)))
            {
                return false;
            }

            var idMembers = await chatMemberships.Where(x => !x.UserId.Equals(container.SenderId)).Select(y => y.UserId).ToListAsync().ConfigureAwait(true);

            if(idMembers.Count == 0)
            {
                if(isChatFriend)
                {
                    return false;
                }

                isOneMember = true;
            }

            int idEjected = container.SenderId;
            NotificationType type = NotificationType.RelationDeletion;
            string extraData = string.Empty;

            if (!container.MemberId.Equals(default))
            {
                if(!idMembers.Contains(container.MemberId) || !chat.OwnerId.Equals(container.SenderId))
                {
                    return false;
                }

                idEjected = container.MemberId;
                isChatFriend = false;
                type = NotificationType.GroupDeletion;
            }

            var sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == idEjected).ConfigureAwait(true);
            var receivers = _context.Users.Where(x => idMembers.Contains(x.Id));

            if (isChatFriend)
            {
                var relations = _context.Friends.Where(x => (x.UserId.Equals(idEjected) && x.FriendId.Equals(idMembers[0]) || (x.UserId.Equals(idMembers[0]) && x.FriendId.Equals(idEjected)))).ToArray();

                if (relations == null)
                {
                    return false;
                }

                _context.Friends.RemoveRange(relations);
                _context.ChatMemberships.RemoveRange(chatMemberships);
            }
            else
            {
                var chatMemberhipToDelete = await chatMemberships.FirstOrDefaultAsync(x => x.UserId.Equals(sender.Id)).ConfigureAwait(true);

                if(!isOneMember)
                {
                    if (sender.Id.Equals(chat.OwnerId))
                    {
                        chat.OwnerId = idMembers[0];
                    }
                }

                if(!type.Equals(NotificationType.GroupDeletion))
                {
                    type = NotificationType.GroupLeaving;
                }

                extraData = chat.Name;

                _context.ChatMemberships.Remove(chatMemberhipToDelete);
            }

            IList<Notification> notifications = new List<Notification>();
            foreach(var user in receivers)
            {
                notifications.Add(new Notification
                {
                    Receiver = user,
                    Sender = sender,
                    Type = type,
                    Desc = container.Reason,
                    ExtraData = extraData,
                    Date = DateTime.Now
                });
            }

            if(notifications != null)
            {
                await _context.Notifications.AddRangeAsync(notifications).ConfigureAwait(true);

                await _context.SaveChangesAsync().ConfigureAwait(true);

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
                    ReceiverId = notification.ReceiverId,
                    Type = notification.Type,
                    Desc = notification.Desc,
                    ExtraData = notification.ExtraData,
                    Date = notification.Date
                } };

                var connectionId = _serivce.GetConnectionId(notification.ReceiverId);
                if(connectionId != null)
                {
                    _ = _hub.Clients.Client(connectionId).SendAsync("transfernotifications", param).ConfigureAwait(true);
                }
            }
        }
    }
}
