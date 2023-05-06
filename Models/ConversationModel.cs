using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biddo.Models
{
    public class ConversationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConversationId { get; set; }

        public int UserId { get; set; }

        public int VendorId { get; set; }

        public int AdminId { get; set; } = 0;

        public bool IsBlocked { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserModel User { get; set; }

        [ForeignKey(nameof(VendorId))]
        public virtual VendorModel Vendor { get; set; }

    }
}
