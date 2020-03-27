using LiveChatRegisterLogin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public interface IUserRepository
    {

        Task<User> Login(string email, string password);
        Task<User> Register(User user, string password);
        Task<ICollection<User>> GetAllFriend(int userId);
        Task<bool> UserExists(string email);
    }
}
