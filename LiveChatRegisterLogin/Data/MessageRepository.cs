using LiveChatRegisterLogin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Containers;

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
            Chat chat;
            User sender;

            sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == senderId).ConfigureAwait(true);
            chat = sender.ChatMemberships.Where(x => x.ChatId == chatId).Select(c => c.Chat).FirstOrDefault();

            return new MessageContainer
            {
                Chat = chat,
                Sender = sender
            };
        }

        public async Task SendMessage(Message message)
        {
            await _context.Messages.AddAsync(message).ConfigureAwait(true);
            await _context.SaveChangesAsync().ConfigureAwait(true);
        }
    }
}
