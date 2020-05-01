using LiveChatRegisterLogin.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.HubConfig
{
    public class ConnectionHub : Hub
    {
        private IConnectionService _service;
        public ConnectionHub(IConnectionService service)
        {
            _service = service;
        }

        public void GetConnectionId(int userId)
        {
            _service.AddConnection(userId, Context.ConnectionId);
        }
    }
}
