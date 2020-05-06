using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Services
{
    public interface IConnectionService
    {
        bool AddConnection(string connectionId, int userId);
        void DisposeConnection(string connectionId);
        bool HasUserId(int userId);
        bool HasConnection(string connectionId);
        string GetConnectionId(int userId);
    }
}
