using Castle.Core.Internal;
using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using LiveChatRegisterLogin.Types;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public class ChatRepository : IChatRepository
    {
        DataContext _context;

        public ChatRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Chat> GetChats(int userId)
        {
            var chats = _context.Chats.Where(x => x.ChatMemberships.Any(y => y.UserId == userId));

            return chats.ToArray();
        }

        public ICollection<ChatDTO> ConvertChats(ICollection<Chat> chats, int userId)
        {
            var chatsDTO = new List<ChatDTO>();

            if(chats == null || chats.Count.Equals(0))
            {
                return chatsDTO;
            }

            foreach(Chat chat in chats)
            {
                string name = string.Empty;
                switch(chat.Type)
                {
                    case Types.ChatType.FriendsChat:
                        name = chat.ChatMemberships.Where(x => x.User.Id != userId).Select(x => x.User).FirstOrDefault().Email;
                        break;
                    case Types.ChatType.GroupChat:
                        name = (chat.Name.IsNullOrEmpty() ? _context.Users.FirstOrDefault(x => x.Id.Equals(chat.OwnerId))?.Email : chat.Name) ?? "Chat without name";
                        break;
                }
                chatsDTO.Add(new ChatDTO
                {
                    Id = chat.Id,
                    Name = name,
                    OwnerId = chat.OwnerId,
                    Type = chat.Type
                });
            }

            return chatsDTO;
        }

        public async Task<Chat> GetChat(int id)
        {
            return await _context.Chats.FirstOrDefaultAsync(x => x.Id.Equals(id)).ConfigureAwait(true);
        }

        public async Task<Chat> CreateChat(ChatType type, int ownerId, string name = "")
        {
            User owner = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(ownerId)).ConfigureAwait(true);
            
            if(owner == null)
            {
                return null;
            }

            return await CreateChat(type, owner, name).ConfigureAwait(true);
        }

        public async Task<Chat> CreateChat(ChatType type, User Owner, string name = "")
        {
            Chat chat = new Chat
            {
                Type = type,
                OwnerId = Owner.Id,
                Name = name.IsNullOrEmpty() ? Owner.Email : name
            };

            if(type.Equals(ChatType.GroupChat))
            {
                await _context.ChatMemberships.AddAsync(new ChatMembership
                {
                    Chat = chat,
                    User = Owner
                }).ConfigureAwait(true);
            }

            await _context.Chats.AddAsync(chat).ConfigureAwait(true);

            await _context.SaveChangesAsync().ConfigureAwait(true);

            return chat;
        }

        public async Task CreateChatMemberships(User requester, User recipient, Chat chat)
        {
            IList<ChatMembership> chatMemberships = new List<ChatMembership>();

            if(chat.Type.Equals(ChatType.FriendsChat))
            {
                chatMemberships.Add(new ChatMembership
                {
                    Chat = chat,
                    User = requester
                });
            }
            
            chatMemberships.Add(new ChatMembership
            {
                Chat = chat,
                User = recipient
            });

            await _context.ChatMemberships.AddRangeAsync(chatMemberships).ConfigureAwait(true);

            await _context.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task<IEnumerable<User>> GetAllMembershipFromChat(int chatId)
        {
            return await _context.ChatMemberships.Where(x => x.ChatId.Equals(chatId)).Select(y => y.User).ToListAsync().ConfigureAwait(true);
        }
    }
}
