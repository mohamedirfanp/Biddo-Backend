using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biddo.Models;
using Microsoft.AspNetCore.Authorization;
using Biddo.Services.Models;

namespace Biddo.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class EventController : ControllerBase
	{
		private readonly BiddoContext _context;
		private readonly IEventService _eventService;

		public EventController(BiddoContext context, IEventService eventService)
		{
			_context = context;
			_eventService = eventService;
		}

		// To Create a new Event 
		[HttpPost("create"), Authorize(Roles = "User")]
		public async Task<IActionResult> AddEvent([FromBody] EventDTO eventDTO)
		{

			var response = this._eventService.AddEvent(eventDTO.Event, eventDTO.SelectedServices);

			if (response is BadRequestObjectResult badRequest)
			{
				return BadRequest(badRequest.Value);
			}

			return Ok(response);
		}

		// To Get Provided Services
		[HttpGet("services")]
		public async Task<ActionResult<IEnumerable<ProvidedServiceModel>>> GetServices()
		{
			var response = this._eventService.GetProvidedServices();

			if(response is BadRequestObjectResult badRequest)
			{
				return BadRequest(badRequest.Value);
			}

			return Ok(response);
		}

		// To Get the List of auction for Vendor
		[HttpGet("auctionEvents"), Authorize(Roles = "Vendor")]
		public async Task<ActionResult<IEnumerable<object>>> ListEvents()
		{
			var response = this._eventService.AuctionEvents();
			return Ok(response);
		}


		// To Get the Event List Based on the filter for User
		[HttpGet("user/ListEvents/{filter}"),Authorize(Roles = "User")]
		public async Task<ActionResult<IEnumerable<object>>> ListEventsForUser(string filter)
		{
			var response = this._eventService.ListEventForUser(filter);

			if (response is BadRequestObjectResult badRequest)
			{
				return BadRequest(badRequest.Value);
			}

			return Ok(response);
		}

		// To Get the Event List Based on the filter for vendor
		[HttpGet("vendor/ListEvent/{filter}"), Authorize(Roles = "Vendor")]
		public async Task<ActionResult<IEnumerable<object>>> ListEventsForVendor(string filter)
		{
			var response = this._eventService.ListEventForVendor(filter);

            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

		// To place a bid for a auction on Vendor
		[HttpPost("vendor/placeBid"), Authorize(Roles = "Vendor")]
		public async Task<IActionResult> AddBid([FromBody] BidDto Bid)
		{
			BiddingModel newBid = new BiddingModel();
			newBid.EventId = Bid.EventId;
			newBid.SelectedServiceName = Bid.SelectedServiceName;
			newBid.Bid = Bid.Bid;
			var response = this._eventService.PlaceBid(newBid);
			if (response is BadRequestObjectResult badRequest)
			{
				return BadRequest(badRequest.Value);
			}

			return Ok(response);
		}

        // To update a bid for a auction on Vendor
        [HttpPost("vendor/updateBid"), Authorize(Roles = "Vendor")]
		public async Task<IActionResult> EditBid([FromBody]BidDto Bid)
		{
			var response = this._eventService.UpdateBid(Bid.BidId, Bid.Bid);
			if (response is BadRequestObjectResult badRequest)
			{
				return BadRequest(badRequest.Value);
			}

			return Ok(response);
		}


		// To update all the over Event automatically
		[HttpGet("update/timeComplete")]
		public async Task<IActionResult> UpdateTimeComplete()
		{
			var response = this._eventService.UpdateCompleted();
			if (response is BadRequestObjectResult badRequest)
			{
				return BadRequest(badRequest.Value);
			}

			return Ok(response);
		}

		// To update the selected vendor for a service
		[HttpPost("update/winner"), Authorize(Roles = "User")]
		public async Task<IActionResult> UpdateWinnerForServices([FromBody] AuctionDto auction)
		{
			var response = this._eventService.UpdateWinnerForService(auction);
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

		// To Reschedule a Auction on User Request
		[HttpPost("reschedule/auction"), Authorize(Roles = "User")]
		public async Task<IActionResult> RescheduleAuction(RescheduleAuctionDto rescheduleRequest)
		{
			var response = this._eventService.RescheduleAuction(rescheduleRequest);
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

		// To add rating for a Vendor on Service
		[HttpPost("add/rating"), Authorize(Roles = "User")]
		public async Task<IActionResult> AddRating(RatingDto newRating)
		{
            var response = this._eventService.AddRating(newRating);
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

	}
}
