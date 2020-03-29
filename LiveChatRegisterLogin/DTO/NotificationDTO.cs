using System.ComponentModel.DataAnnotations;

namespace LiveChatRegisterLogin.DTO
{
    public class NotificationDTO
    {
        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "'0' Can not be used")]
        public string Id { get; set; }
        
        [Required]
        public bool Action { get; set; }
    }
}
