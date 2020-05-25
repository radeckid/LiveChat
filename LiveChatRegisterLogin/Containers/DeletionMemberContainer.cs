using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Containers
{
    public class DeletionMemberContainer
    {
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public string Reason { get; set; }
        public int MemberId { get; set; }
    }
}
