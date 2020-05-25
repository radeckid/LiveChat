using System.ComponentModel.DataAnnotations;

namespace LiveChatRegisterLogin.DTO
{
    public class RelationDeletionDTO
    {
        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ChatId { get; set; }

        public string Reason { get; set; }

        public string MemberId { get; set; }
    }
}
