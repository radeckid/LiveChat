using System.ComponentModel.DataAnnotations;

namespace LiveChatRegisterLogin.DTO
{
    public class ChatDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
