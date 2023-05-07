namespace Biddo.Services.Models
{
    public class RatingDto
    {
        public int EventId { get; set; }

        public int SelectedServiceId { get; set; }

        public string ServiceName { get; set; }  


        public int VendorId { get; set; }

        public string Review { get; set; }

        public int Rating { get; set; }

    }
}
