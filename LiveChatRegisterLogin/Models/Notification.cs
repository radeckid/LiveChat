using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }

        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        public string Desc { get; set; }

        public string ExtraData { get; set; }

        public DateTime Date { get; set; }
    }
}
