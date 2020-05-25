using LiveChatRegisterLogin.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Containers
{
    public class InvitationContainer
    {
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public int ChatId { get; set; }
        public NotificationType Type { get; set; }
    }
}
