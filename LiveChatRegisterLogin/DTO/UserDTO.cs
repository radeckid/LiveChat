using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.DTO
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(20, ErrorMessage = "Max 64")]
        [MinLength(3, ErrorMessage = "Max 3")]
        public string Email { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Max 25")]
        [MinLength(3, ErrorMessage = "Max 3")]
        public string Password { get; set; }
    }
}
