using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.HubConfig
{
    public class NotificationHub : Hub
    {
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
