using LiveChatRegisterLogin.Containers;
using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public interface IMessagesRepository
    {
        Task<MessageContainer> GetMessageCointainer(int senderId, int chatId);
        Task<IEnumerable<Models.Message>> GetMessageCointainer(GetMessagesDTO getMessages);
        Task SendMessage(Models.Message message);
    }
}
