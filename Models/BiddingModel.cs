
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biddo.Models
{
    public class BiddingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BidId { get; set; }

        
        public string SelectedServiceName { get; set; }

        public int EventId { get; set; }

        public int Bid { get; set; }

        public int VendorId { get; set; }


        [ForeignKey(nameof(EventId))]
        public virtual EventModelTable Event { get; set; }

        [ForeignKey(nameof(VendorId))]
        public virtual VendorModel Vendor { get; set; }

        public bool IsAuctionCompleted { get; set; }

        public bool win { get; set; }

    }
}
