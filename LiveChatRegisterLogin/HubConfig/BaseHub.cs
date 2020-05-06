using LiveChatRegisterLogin.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.HubConfig
{
    public class BaseHub : Hub
    {
        private IConnectionService _service;

        public BaseHub(IConnectionService service)
        {
            _service = service;
        }

        public bool GetConnectionId(int userId)
        {
            if(_service.HasUserId(userId))
            {
                return false;
            }
            
            _service.AddConnection(Context.User.Identity.Name, userId);
            return true;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string connectionid = Context.ConnectionId;
            if (_service.HasConnection(connectionid))
            {
                _service.DisposeConnection(connectionid);
                Context.Abort();
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
