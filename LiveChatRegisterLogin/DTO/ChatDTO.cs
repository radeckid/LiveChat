using LiveChatRegisterLogin.Types;
using System.ComponentModel.DataAnnotations;

namespace LiveChatRegisterLogin.DTO
{
    public class ChatDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ChatType Type { get; set; }
    }
}
