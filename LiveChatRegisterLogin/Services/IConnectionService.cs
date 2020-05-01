using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Services
{
    public interface IConnectionService
    {
        void AddConnection(int userId, string connectionId);
        void DisposeConnection(int userId);
    }
}
