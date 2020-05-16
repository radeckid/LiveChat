using LiveChatRegisterLogin.Services;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;
using Hub = Microsoft.AspNetCore.SignalR.Hub;

namespace LiveChatRegisterLogin.HubConfig
{
    [Authorize]
    public abstract class BaseHub : Hub
    {
        protected abstract ServiceTypes Type { get; }

        private IConnectionService _connectionService;

        public BaseHub(Func<ServiceTypes, IConnectionService> servicesResolver)
        {
            _connectionService = servicesResolver(Type);
        }

        public override Task OnConnectedAsync()
        {
            if(int.TryParse(Context.UserIdentifier, out int userId))
            {
                _connectionService.AddConnection(Context.ConnectionId, userId);

                return base.OnConnectedAsync();
            }

            throw new Microsoft.AspNetCore.SignalR.HubException("Cannot parse user id");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connectionService.DisposeConnection(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
