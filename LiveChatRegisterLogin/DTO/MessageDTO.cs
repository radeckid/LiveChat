using System;
using System.ComponentModel.DataAnnotations;

namespace LiveChatRegisterLogin.DTO
{
    public class MessageDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "Min 1")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "'0' Can not be used")]
        public string SenderId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Min 1")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "'0' Can not be used")]
        public string ChatId { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Wrong DateTime format")]
        public string Date { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Min 1")]
        public string Content { get; set; }
    }
}
