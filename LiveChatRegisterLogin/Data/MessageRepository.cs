using LiveChatRegisterLogin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Containers;
using LiveChatRegisterLogin.DTO;

namespace LiveChatRegisterLogin.Data
{
    public class MessageRepository : IMessagesRepository
    {
        private readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            if (context != null)
            {
                _context = context;
                _context.Database.EnsureCreated();
            }
        }

        public async Task<MessageContainer> GetMessageCointainer(int senderId, int chatId)
        {
            Chat chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id == chatId && x.ChatMemberships.Any(y => y.UserId == senderId)).ConfigureAwait(true);
            User sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == senderId).ConfigureAwait(true);

            return new MessageContainer
            { 
                Chat = chat,
                Sender = sender
            };
        }

        public async Task<IEnumerable<Message>> GetMessageCointainer(GetMessagesDTO getMessages)
        {
            IList<Message> messages = new List<Message>();

            Chat chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id == getMessages.ChatId && x.ChatMemberships.Any(y => y.UserId == getMessages.RequesterId)).ConfigureAwait(true);
            User sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == getMessages.RequesterId).ConfigureAwait(true);

            if(chat == null || sender == null)
            {
                return null;
            }

            var messagesToProccess = _context.Messages.Where(x => x.ChatId == getMessages.ChatId).ToList();

            if (messagesToProccess == null || messagesToProccess.Count == 0)
            {
                return messages;
            }

            if (getMessages.IdLastMessage == 0)
            {
                getMessages.IdLastMessage = messagesToProccess.LastOrDefault().ChatId;
            }

            int iterator = 0;
            var messageWithCorrectedId = messagesToProccess.Where(x => x.Id < getMessages.IdLastMessage).ToList();

            for(int i = messageWithCorrectedId.Count - 1; i >= 0; i--)
            {
                if(messageWithCorrectedId[i] == null)
                {
                    break;
                }

                messages.Add(messageWithCorrectedId[i]);

                iterator++;

                if (iterator >= 20)
                {
                    break;
                }
            }

            return messages.Reverse();
        }

        public async Task SendMessage(Message message)
        {
            await _context.Messages.AddAsync(message).ConfigureAwait(true);
            await _context.SaveChangesAsync().ConfigureAwait(true);
        }
    }
}
