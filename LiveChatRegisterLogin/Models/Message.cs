﻿using System;
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
        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        [Required]
        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }

        public DateTime Date { get; set; }
        
        public string Content { get; set; }
    }
}
