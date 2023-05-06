namespace Biddo.Services.Models
{
    public class AuctionDto
    {
        public int EventId { get; set; }

        public int SelectedServiceId { get; set; }

        public int BidId { get; set; }

        public int AuctionId { get; set; } = default;

        public int VendorId { get; set; }

    }
}
