using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using System.Collections.Generic;

namespace LiveChatRegisterLogin.Data
{
    public interface IChatRepository
    {
        ICollection<Chat> GetChats(int userId);
        ICollection<ChatDTO> ConvertChats(ICollection<Chat> chats, int userId);
    }
}
