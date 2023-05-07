using Biddo.Models;

namespace Biddo.Services.Models
{
    public class ListEventDto
    {
        public SelectedServiceModel selectedService { get; set; }

        public BiddingModel bidding { get; set; } = null;

        public List<BiddingModel> BidList { get; set; } = null;

        public List<object> RatingList { get; set; } = null;

        public object vendorList { get; set; } = null;

        public bool ratingStatus { get; set; } = false;
    }
}
