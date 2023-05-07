using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biddo.Models
{
    public class TimelineCommentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public int TimelineCommentId { get; set; }

        [Required]
        public string message { get; set; }

        [Required]
        public int From { get; set; } 

        public DateTime TimeStamp { get; set; }

        public int ConversationId { get; set; }

        public int QueryId { get; set; }

        public string FromRole { get; set; }




    }
}
