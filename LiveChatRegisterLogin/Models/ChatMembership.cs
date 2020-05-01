

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveChatRegisterLogin.Models
{
    public class ChatMembership
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
