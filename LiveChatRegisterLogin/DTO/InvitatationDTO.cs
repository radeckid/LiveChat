using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
