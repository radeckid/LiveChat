using System;

namespace LiveChatRegisterLogin.HubConfig
{
    public class MessageChartModel
    {
        public int Id { get; set; }

        public string SenderName { get; set; }

        public int ChatId { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }
    }
}
