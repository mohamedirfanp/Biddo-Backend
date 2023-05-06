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

		[HttpGet("auctionEvents"), Authorize(Roles = "Vendor")]
		public async Task<ActionResult<IEnumerable<object>>> ListEvents()
		{
			var response = this._eventService.AuctionEvents();
			return Ok(response);
		}

		[HttpGet("user/ListEvents"),Authorize(Roles = "User")]

		public async Task<ActionResult<IEnumerable<object>>> ListEventsForUser()
		{
			var response = this._eventService.ListEventForUser();

			if (response is BadRequestObjectResult badRequest)
			{
				return BadRequest(badRequest.Value);
			}

			return Ok(response);
		}

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

	}
}
