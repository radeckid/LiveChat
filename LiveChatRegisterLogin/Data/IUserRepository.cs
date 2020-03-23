using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public interface IUserRepository
    {

        Task<User> Login(string email, string password);
        Task<User> Register(User user, string password);
        Task<bool> UserExists(string email);
    }
}
