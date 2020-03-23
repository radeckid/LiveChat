using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public string ReciverId { get; set; }

        [Required]
        [MaxLength(5, ErrorMessage = "Max 5 lenght")]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "Time must be between 00:00 to 23:59")]
        public string Time { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Min 1")]
        public string Content { get; set; }
    }
}
