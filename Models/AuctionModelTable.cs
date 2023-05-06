using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Biddo.Models
{
    public class AuctionModelTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuctionID { get; set; }

        public int EventId { get; set; }

        public int VendorId { get; set; }
        public int BidId { get; set; }
        public int SelectedServiceId { get; set; }


        [ForeignKey(nameof(BidId))]
        public virtual BiddingModel Bid { get; set; } = null!;


        [ForeignKey(nameof(SelectedServiceId))]
        public virtual SelectedServiceModel SelectedService { get; set; } = null;
    }
}
