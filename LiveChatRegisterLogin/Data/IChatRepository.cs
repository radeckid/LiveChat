using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using LiveChatRegisterLogin.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public interface IChatRepository
    {
        Task<Chat> CreateChat(ChatType type, int ownerId, string name = "");
        Task<Chat> CreateChat(ChatType type, User owner, string name = "");
        Task<Chat> GetChat(int chatId);
        Task CreateChatMemberships(User requester, User recipient, Chat chat);
        ICollection<Chat> GetChats(int userId);
        ICollection<ChatDTO> ConvertChats(ICollection<Chat> chats, int userId);
        Task<IEnumerable<User>> GetAllMembershipFromChat(int chatId);
    }
}
