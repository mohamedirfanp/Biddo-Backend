using Biddo.Models;
using Biddo.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Biddo.Services.EventServices
{
    public interface IEventService
    {
        // To Create A new Event (User)
        IActionResult AddEvent(EventModelTable Event, List<SelectedServiceModel> selectedServices);

        // To Get the ProvidedServices
        IAsyncEnumerable<ProvidedServiceModel> GetProvidedServices();

        // To Get the Current Auctions for Current Vendor
        IEnumerable<object> AuctionEvents();

        // To Get the List of Events for current User
        IEnumerable<object> ListEventForUser();

        // To Place Bid for a Service (Vendor)
        IActionResult PlaceBid(BiddingModel bid);

        // To Update Bid for a Service(Vendor)
        IActionResult UpdateBid(int bidId, int bid);

        //IActionResult UpdateAuctionOverEvent();

        // To Update Dead Events and Auctions
        IActionResult UpdateCompleted();

        // To Update the Vendor for a Service (User)
        IActionResult UpdateWinnerForService(AuctionDto actionDetail);

        // To Reschedule a Auction (User)
        IActionResult RescheduleAuction(RescheduleAuctionDto rescheduleRequest);

    }
}
