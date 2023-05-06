using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biddo.Models
{
    public class EventModelTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }


        [Required]
        [StringLength(50)]
        public string EventName { get; set; }

        [Required]
        [StringLength(150)]
        public string EventDescription { get; set; }

        [Required]
        [StringLength(50)]
        public string EventAddress { get; set; }


        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(50)]
        public string EventTime { get; set; }

        [Required]
        public int TotalParticipates { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsCompleted { get; set; }

        public int UserId { get; set; }

        public bool IsAuctionCompleted { get; set; }

        public DateTime AuctionTimeLimit { get; set; }

    }
}
