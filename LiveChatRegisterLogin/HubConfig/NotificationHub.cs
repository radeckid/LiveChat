using LiveChatRegisterLogin.Services;
using System;

namespace LiveChatRegisterLogin.HubConfig
{
    public class NotificationHub : BaseHub
    {
        protected override ServiceTypes Type => ServiceTypes.NotificationConnectionService;

        public NotificationHub(Func<ServiceTypes, IConnectionService> servicesResolver) : base(servicesResolver)
        {
           
        }
    }
}
