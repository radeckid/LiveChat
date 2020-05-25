using LiveChatRegisterLogin.Services;
using System;

namespace LiveChatRegisterLogin.HubConfig
{
    public class MessagesHub : BaseHub
    {
        protected override ServiceTypes Type => ServiceTypes.MessageConnectionService;

        public MessagesHub(Func<ServiceTypes, IConnectionService> servicesResolver) : base(servicesResolver)
        {

        }
    }
}
