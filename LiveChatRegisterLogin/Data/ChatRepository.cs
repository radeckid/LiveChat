using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            foreach(Chat chat in chats)
            {
                switch(chat.Type)
                {
                    case Types.ChatType.FriendsChat:
                        chatsDTO.Add(new ChatDTO
                        {
                            Id = chat.Id,
                            Name = chat.ChatMemberships.Where(x => x.User.Id != userId).Select(x => x.User).FirstOrDefault().Email
                        });
                        break;
                    case Types.ChatType.GroupChat:
                        chatsDTO.Add(new ChatDTO
                        {
                            Id = chat.Id,
                            Name = GetNameForGroup(chat.ChatMemberships)
                        });
                        break;
                }
            }

            return chatsDTO;
        }

        private string GetNameForGroup(ICollection<ChatMembership> chatMemberships)
        {
            StringBuilder nameBuilder = new StringBuilder();

            foreach(var member in chatMemberships)
            {
                nameBuilder.Append(member.User.Email);
                nameBuilder.Append(", ");
            }

            return nameBuilder.ToString();
        }
    }
}
