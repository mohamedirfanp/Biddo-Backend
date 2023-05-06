
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biddo.Models
{
    public class QueryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QueryId { get; set; }

        [Required]
        [StringLength(50)]
        public string QueryTitle { get; set; }

        [Required]
        [StringLength(30)]
        public string QueryType { get; set; }

        [Required]
        public string QueryDesciption { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public string Response { get; set; }

        public DateTime ResponseAt { get; set; }

        public string Status { get; set; }


        public int ConversationId { get; set; }

        [ForeignKey(nameof(ConversationId))]
        public virtual ConversationModel Conversation { get; set; } 

        [ForeignKey(nameof(UserId))]
        public virtual UserModel User { get; set; }

    }
}
