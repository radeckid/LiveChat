using LiveChatRegisterLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Containers
{
    public struct MessageContainer
    {
        public Chat Chat { get; set; }
        public User Sender { get; set; }

        internal void Deconstruct(out Chat chat, out User user)
        {
            chat = Chat;
            user = Sender;
        }
    }
}
