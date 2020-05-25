using LiveChatRegisterLogin.Types;
using System;
using System.Collections.Generic;

namespace LiveChatRegisterLogin.HubConfig
{
    public class NotificationChartModel
    {
        public int Id { get; set; }

        public int ReceiverId { get; set; }

        public string SenderEmail { get; set; }

        public NotificationType Type { get; set; }

        public string Desc { get; set; }

        public string ExtraData { get; set; }

        public DateTime Date { get; set; }
    }
}
