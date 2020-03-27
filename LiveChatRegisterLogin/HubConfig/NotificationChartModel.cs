using LiveChatRegisterLogin.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.HubConfig
{
    public class NotificationChartModel
    {
        public int ReceiverId { get; set; }

        public int SenderId { get; set; }

        public NotificationType Type { get; set; }

        public string Desc { get; set; }

        public string ExtraData { get; set; }

        public DateTime Date { get; set; }
    }
}
