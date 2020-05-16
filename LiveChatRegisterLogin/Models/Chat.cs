using LiveChatRegisterLogin.Types;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LiveChatRegisterLogin.Models
{
    public class Chat
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public ChatType Type { get; set; } 

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<ChatMembership> ChatMemberships { get; set; }
    }
}
