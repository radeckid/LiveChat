using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LiveChatRegisterLogin.Models
{
    [Serializable]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [JsonIgnore]
        [field: NonSerialized]
        public virtual Password Password { get; set; }

        [JsonIgnore]
        [field: NonSerialized]
        public virtual ICollection<Relation> Friends { get; set; }

        [JsonIgnore]
        [field: NonSerialized]
        public virtual ICollection<ChatMembership> ChatMemberships { get; set; }

        [JsonIgnore]
        [field: NonSerialized]
        public virtual ICollection<Notification> NotificationsSent { get; set; }

        [JsonIgnore]
        [field: NonSerialized]
        public virtual ICollection<Notification> NotificationsReceived { get; set; }
    }
}
