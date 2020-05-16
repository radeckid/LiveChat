using LiveChatRegisterLogin.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveChatRegisterLogin.Models
{
    [Serializable]
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Receiver")]
        [JsonIgnore]
        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }

        [ForeignKey("Sender")]
        [JsonIgnore]
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        public NotificationType Type { get; set; }

        public string Desc { get; set; }

        public string ExtraData { get; set; }

        public DateTime Date { get; set; }
    }
}
