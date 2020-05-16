using LiveChatRegisterLogin.Types;
using System.ComponentModel.DataAnnotations;

namespace LiveChatRegisterLogin.DTO
{
    public class InvitatationDTO
    {
        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "'0' Can not be used")]
        public string UserId { get; set; }

        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "'0' Can not be used")]
        public string OtherId { get; set; }

        public string ChatId { get; set; }
    }
}
