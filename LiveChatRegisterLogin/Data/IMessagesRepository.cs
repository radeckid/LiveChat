using LiveChatRegisterLogin.Containers;
using LiveChatRegisterLogin.Models;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public interface IMessagesRepository
    {
        Task<MessageContainer> GetMessageCointainer(int senderIdDTO, int chatidDTO);
        Task SendMessage(Message message);
    }
}
