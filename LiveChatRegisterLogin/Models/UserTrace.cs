using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Models
{
    public class UserTrace
    {
        public int Id { get; set; }

        public string Who { get; set; }

        public int UserIdProvided { get; set; }

        public DateTime When { get; set; }

        public bool Result { get; set; }

        public string IpAddress { get; set; }
    }
}
