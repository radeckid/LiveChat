using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int SenderIdFK { get; set; }

        [Required]
        [ForeignKey("User")]
        public int ReciverIdFK { get; set; }

        public string Time { get; set; }
        
        public string Content { get; set; }
    }
}
