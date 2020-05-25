using LiveChatRegisterLogin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveChatRegisterLogin
{
    public class Password
    {
        [Key]
        public int Id {get; set;}

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; } 
    }
}
