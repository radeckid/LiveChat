using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LiveChatRegisterLogin.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Sender")]
        [JsonIgnore]
        public int SenderId { get; set; }
        [JsonIgnore]
        public virtual User Sender { get; set; }

        public string SenderName { get; set; }

        [Required]
        [ForeignKey("Chat")]
        [JsonIgnore]
        public int ChatId { get; set; }
        [JsonIgnore]
        public virtual Chat Chat { get; set; }

        public DateTime Date { get; set; }
        
        public string Content { get; set; }
    }
}
