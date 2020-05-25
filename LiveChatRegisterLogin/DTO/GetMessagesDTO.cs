using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.DTO
{
    public class GetMessagesDTO
    {
        public int IdLastMessage { get; set; }

        public int ChatId { get; set; }

        public int RequesterId { get; set; }
    }
}
