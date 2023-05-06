namespace Biddo.Services.Models
{
    public class BidDto
    {
        public int EventId { get; set; } = 0;

        public string SelectedServiceName { get; set; } = string.Empty;

        public int Bid { get; set; } = 0;

        public int BidId { get; set; } = 0;
    }
}
