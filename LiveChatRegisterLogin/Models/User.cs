using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LiveChatRegisterLogin.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [JsonIgnore]
        public virtual Password Password { get; set; }

        [JsonIgnore]
        public virtual ICollection<Message> SentMessages { get; set; }

        [JsonIgnore]
        public virtual ICollection<Message> ReceivedMessages { get; set; }

        [JsonIgnore]
        public virtual ICollection<Relation> Friends { get; set; }

        [JsonIgnore]
        public virtual ICollection<Notification> NotificationsSent { get; set; }

        [JsonIgnore]
        public virtual ICollection<Notification> NotificationsReceived { get; set; }
    }
}
