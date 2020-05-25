using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.Models;
using LiveChatRegisterLogin.Types;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Helpers
{
    public class NotificationProcess
    {
        private Notification _notification;
        private User _requester;
        private User _currentUser;
        private NotificationType _type;

        private bool _isAccepted;
        private IChatRepository _repository;

        public Notification InformationNotification { get; set; }

        public NotificationProcess(IChatRepository repository, Notification notification, bool isAccepted)
        {
            _repository = repository;
            _notification = notification;
            _requester = notification.Sender;
            _currentUser = notification.Receiver;
            _type = notification.Type;
            _isAccepted = isAccepted;
            _repository = repository;
        }

        public async Task Process()
        {
            bool isFriendInvitation = _type.Equals(NotificationType.Invitation);
            bool isGroupInvitation = _type.Equals(NotificationType.GroupInvitation);
            string notificationDesc = string.Empty;

            if (isFriendInvitation || isGroupInvitation)
            {
                if (_isAccepted)
                {
                    notificationDesc = await UpdateOrCreateChat().ConfigureAwait(true);
                }
                else
                {
                    if(isFriendInvitation)
                    {
                        notificationDesc = $"User {_currentUser.Email} rejected your invitation.";
                    }
                    else
                    {
                        string chatName = (await GetChat().ConfigureAwait(true))?.Name;
                        notificationDesc = $"User {_currentUser.Email} rejected your invitation about: {chatName}.";
                    }
                }

                InformationNotification = new Notification
                {
                    Sender = _currentUser,
                    Receiver = _requester,
                    Type = NotificationType.Information,
                    Desc = notificationDesc,
                    ExtraData = string.Empty,
                    Date = DateTime.Now
                };
            }
        }

        private async Task<string> UpdateOrCreateChat()
        {
            Chat chat = null;
            string notificationDesc = string.Empty;

            if (_type.Equals(NotificationType.Invitation))
            {
                await AddFriend().ConfigureAwait(true);

                chat = await _repository.CreateChat(ChatType.FriendsChat, _requester).ConfigureAwait(true);
                notificationDesc = $"User {_currentUser.Email} accepted your invitation.";
            }
            else
            {
                chat = await GetChat().ConfigureAwait(true);

                if(chat == null)
                {
                    throw new ArgumentNullException("Chat is null");
                }

                notificationDesc = $"User {_currentUser.Email} accepted invitation to {chat.Name} chat.";
            }

            await _repository.CreateChatMemberships(_requester, _currentUser, chat).ConfigureAwait(true);

            return notificationDesc;
        }

        private async Task<Chat> GetChat()
        {
            if (!int.TryParse(_notification.ExtraData, out int chatId))
            {
                throw new ArgumentException("Cannot parse chat of from Extra data");
            }

            return await _repository.GetChat(chatId).ConfigureAwait(true); 
        }

        private async Task AddFriend()
        {
            _requester.Friends.Add(new Relation
            {
                FriendId = _currentUser.Id
            });
            _currentUser.Friends.Add(new Relation
            {
                FriendId = _requester.Id
            });
        }
    }
}
