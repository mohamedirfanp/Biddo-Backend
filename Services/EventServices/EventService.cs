using Biddo.Models;
using Biddo.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Biddo.Services.EventServices
{
    public class EventService : IEventService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly BiddoContext _context;
		private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public EventService(BiddoContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IMailService mailService)
		{
			_httpContextAccessor = httpContextAccessor;
			_context = context;
			_configuration = configuration;
			_mailService = mailService;

		}

		// To Get the Current User ID
        public int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId);
        }

		// To Post the new Event
        public IActionResult AddEvent(EventModelTable Event, List<SelectedServiceModel> selectedServices)
		{
			if (_context.EventModelTable == null)
			{
				return new NotFoundObjectResult("Event Table Not Found");
			}

			try
			{
				var userId = GetCurrentUserId();

		
				Event.IsCompleted = false;
				Event.UserId = userId;
				Event.IsDeleted = false;
				Event.IsAuctionCompleted = false;



				// NEED : Change the Auction Time in production
				Event.AuctionTimeLimit = DateTime.Now.AddMinutes(10);


				_context.EventModelTable.Add(Event);
				_context.SaveChanges();

				var eventId = Event.EventId;
				foreach (var selectedService in selectedServices)
				{
					selectedService.EventId = eventId;


					_context.SelectedServiceTable.Add(selectedService);

					_context.SaveChanges();
				}

				string subject = "A New Event Arrived";

					SendMailForVendors(subject,Event, selectedServices);

				return new OkObjectResult("Successfully Event Created");
			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}
		}

		// To Get the List of Service Provided in this Platform
        public async IAsyncEnumerable<ProvidedServiceModel> GetProvidedServices()
        {
            var distinctServices = await _context.ProvidedServiceTable
                .GroupBy(p => p.ServiceName)
                .Select(g => g.FirstOrDefault())
                .ToListAsync();

            foreach (var service in distinctServices)
            {
                yield return service;
            }
        }

		// TO GET the List of Event For Current User
        public IEnumerable<object> ListEventForUser()
		{
			int userId = GetCurrentUserId();


            var arrayOfObjects = new List<object>();
            try
            {
				var eventsList = _context.EventModelTable.Where(e => e.UserId ==  userId).ToList();
				
                foreach (var e in eventsList)
                {
                    dynamic tempObj = new System.Dynamic.ExpandoObject();

                    tempObj.Event = e;
                    tempObj.Services = new List<ListEventDto>();


                    var selectedServices = _context.SelectedServiceTable.Where(service => service.EventId == e.EventId).ToList();

                    foreach (var service in selectedServices)
                    {
						List<BiddingModel> selectedServicesBidList = new List<BiddingModel>();
						var vendor = new object();
						var vendorList = new List<object>();
                        var biddedList = _context.BiddingTable.Where(bid => bid.SelectedServiceName == service.SelectServiceName && bid.EventId == e.EventId).ToList();
                        ListEventDto tempDto = new ListEventDto();

                        if (biddedList != null)
                        {
                                tempDto.selectedService = service;
                            foreach (var bid in biddedList)
                            {
                                vendor = _context.VendorModel
										.Where(v => v.VendorId == bid.VendorId)
											.Select(v => new
											{
												v.VendorName,
												v.VendorEmail,
												v.VendorPhoneNumber,
												v.VendorLocation
											})
									  .FirstOrDefault();
								if(bid.win == true)
								{
									vendorList.Clear();
									vendorList.Add(vendor);
									selectedServicesBidList.Clear();
									selectedServicesBidList.Add(bid);
									break;
								}

								selectedServicesBidList.Add(bid);

								vendorList.Add(vendor);

                            }

                            //tempDto.vendor = vendor;
							
                            tempDto.BidList = selectedServicesBidList;
                            tempDto.vendorList = vendorList;
                            tempObj.Services.Add(tempDto);

                        }
                    }


                    arrayOfObjects.Add(tempObj);

                }

                return arrayOfObjects;
            }
            catch (Exception ex)
            {
                return new List<object>().Append(ex.Message);
            }

        }

		// TO Get the List of Auction Event For Current Vendor
		public IEnumerable<object> AuctionEvents()
		{
			// TODO : GetCurrentUserId
			int vendorId = GetCurrentUserId();

			var vendorProvidedService = _context.ProvidedServiceTable.Where(service => service.VendorId == vendorId).ToList();
			var eventsFromToday = ListEventsWithParams(DateTime.Now);

			var arrayOfObjects = new List<object>();
			try
			{
				foreach (var e in eventsFromToday)
			{


				dynamic tempObj = new System.Dynamic.ExpandoObject();

				tempObj.Event = e;
				tempObj.Services = new List<ListEventDto>();


				var selectedServices = _context.SelectedServiceTable.Where(service => service.EventId == e.EventId && service.IsServiceCompleted == false).ToList();

                foreach (var service in selectedServices)
                {
					var alreadyBidded = false;
					var biddedList = _context.BiddingTable.Where(bid => bid.SelectedServiceName == service.SelectServiceName && bid.EventId == e.EventId && bid.VendorId == vendorId).FirstOrDefault();

                    if(biddedList != null)
					{
						alreadyBidded = true;
					}

                    foreach (var providedService in vendorProvidedService)
                    {
                        if(providedService.ServiceName == service.SelectServiceName)
						{
							ListEventDto tempDto = new ListEventDto();
							tempDto.selectedService = service;

							
							if(alreadyBidded)
							{
								tempDto.bidding = biddedList;
							}

							tempObj.Services.Add(tempDto);

                        }
                    }
                }

				if(tempObj.Services.Count > 0)
					{

                arrayOfObjects.Add(tempObj);
					}



            }

				return arrayOfObjects;
			}
			catch(Exception ex)
			{
				return new List<object>().Append(ex.Message);
			}

    

        }

		public IActionResult PlaceBid(BiddingModel bid)
		{
			Console.WriteLine(bid.Bid);
            Console.WriteLine(bid.EventId);
            Console.WriteLine(bid.SelectedServiceName);
            if (_context.BiddingTable == null)
            {
				return new BadRequestObjectResult("Unknown Error");
            }

			try
			{
				int vendorId = GetCurrentUserId();
				bid.VendorId = vendorId;
				_context.BiddingTable.Add(bid);
				_context.SaveChanges();

				return new OkObjectResult("Successfully Placed");

			}
			catch (Exception ex) { 
				Console.WriteLine(ex);
				return new BadRequestObjectResult(ex.Message);
			}
        }

		public IActionResult UpdateBid(int bidId, int bid)
		{
            if (_context.BiddingTable == null)
            {
                return new BadRequestObjectResult("Unknown Error");
            }
   
            try
            {
				var oldBid = _context.BiddingTable.Where(b => b.BidId == bidId).FirstOrDefault();
				if (oldBid == null)
				{
                    return new BadRequestObjectResult("Bid Not Placed Error");
                }
				oldBid.Bid = bid;
               
                _context.SaveChanges();

                return new OkObjectResult("Successfully Placed");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }

		//public IActionResult AddRating(RatingModel rating)
		//{

		//}

		// TODO : UnComment The Auction Time Update
		public IActionResult UpdateCompleted()
		{
			try
			{
				var eventListBeforeToday = _context.EventModelTable.Where(e => e.EventDate < DateTime.Now && e.IsCompleted == false).ToList();
                foreach (var e in eventListBeforeToday)
                {
					e.IsCompleted = true;
                }
				_context.SaveChanges();

				var eventList = _context.EventModelTable.Where(e => e.AuctionTimeLimit < DateTime.Now).ToList();
				foreach (var e in eventList)
				{
					e.IsAuctionCompleted = true;
                }
				_context.SaveChanges();

				return new OkObjectResult("Successfully Updated Dead Events");
            }
			catch(Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}
		}

		// Function to Update the Winner(Vendor) for the Selected Service. 
		public IActionResult UpdateWinnerForService(AuctionDto auction)
		{
			try
			{
				var currentBid = _context.BiddingTable.Include(bid => bid.Vendor).Where(bid => bid.BidId == auction.BidId).FirstOrDefault();

				if(currentBid != null)
				{
					currentBid.win = true;
					_context.SaveChanges();
				}

				var selectedService = _context.SelectedServiceTable.Where(service => service.SelectedServiceId == auction.SelectedServiceId).FirstOrDefault();


				if(selectedService != null)
				{
					selectedService.IsServiceCompleted = true;
					_context.SaveChanges();
				}

				//// Update the Event IsComplete
				//UpdateEventStatus(auction.EventId);


				// To Send Confirmed Email to Choosen Vendor
				MailDto mailDto = new MailDto();
				mailDto.Subject = "Congrats!! Bid Confirmation Mail";

				var body = $"<h2>Congrations</h2><br><p>Your Bid For Service : {selectedService?.SelectServiceName} is accepted</p><br> Accepted Price : {currentBid?.Bid} <br>" +
                    $"The User will Contact You Shortly in the Chat Section<br>" +
                    $"Login to the site to Contact the User in the Chat Section<br>" +
                    $"Regards<br>" +
                    $"Biddo";
				mailDto.Body = body;

				List<string> vendorList = new List<string>
				{
					currentBid.Vendor.VendorEmail
				};

				mailDto.ToList = vendorList;

				_mailService.SendMail(mailDto);

				// Create a Conversation between Them.
				int currentId = GetCurrentUserId();

				// Check If the Already a Conversation
				var existingConversation = _context.ConvensationTable.Where(conv => conv.VendorId == auction.VendorId && conv.UserId ==currentId).FirstOrDefault();

				// Create a Chat
				TimelineCommentModel timelineCommentModel = new TimelineCommentModel();
				timelineCommentModel.From = currentId;
				timelineCommentModel.TimeStamp = DateTime.Now;
				timelineCommentModel.message = $"<p>Requested Service Name : {selectedService.SelectServiceName}</p><br>" +
					$"<p>Accepted Price : {currentBid.Bid}</p><br>";
				timelineCommentModel.FromRole = "User";

				if(existingConversation == null)
				{
					ConversationModel newConversation = new ConversationModel();
					newConversation.UserId = currentId;
					newConversation.VendorId = currentBid.VendorId;

					_context.ConvensationTable.Add(newConversation);
					_context.SaveChanges();

					timelineCommentModel.ConversationId = newConversation.ConversationId;
					
				}
				else
				{
					timelineCommentModel.ConversationId = existingConversation.ConversationId;

				}

                _context.TimelineCommentModel.Add(timelineCommentModel);

                _context.SaveChanges();


                return new OkObjectResult("Successfully Added");

			}
			catch (Exception ex)
			{
				
				return new BadRequestObjectResult(ex.Message);
			}
		}

        // Helper Function

		// To Get the List of Events based on Constraints
        private IEnumerable<EventModelTable> ListEventsWithParams(DateTime today)
		{
			try
			{
				var eventsList = _context.EventModelTable.Where(e => e.EventDate >= today && !e.IsAuctionCompleted).ToList();

				return eventsList;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return new List<EventModelTable>();
			}
		}


		// To Send Mail for Vendor about the Event
		public void SendMailForVendors(string subject,EventModelTable eventModel, List<SelectedServiceModel> selectedServices)
		{
			try
			{
                foreach (var service in selectedServices)
                {
					var vendorList = _context.ProvidedServiceTable.Include(p => p.Vendor).Select(p=> new { p.Vendor.VendorEmail, p.ServiceName }).Where(p => p.ServiceName == service.SelectServiceName).ToList();

					string body = $"Event Name : {eventModel.EventName}<br>" + $"Event Date & Time : {eventModel.EventDate.ToString().Split('T')[0]} & {eventModel.EventTime}<br>" + $"Event Venue : {eventModel.EventAddress}<br>" + $"Requested Service Name : {service.SelectServiceName}<br>" + $"For Further Info, login to the site.<br>" + 
						$"<h3 style='color : red;'>The auction over at {eventModel.AuctionTimeLimit}.So, please login before the time and placed your bid in Auction Section<h3><br>" +
						$"Regards<br>" +
						$"Biddo";

					MailDto mailDto = new MailDto();
					mailDto.Subject = subject;
					mailDto.Body = body;
					mailDto.ToList = vendorList.ConvertAll(vendor => vendor.VendorEmail).ToList();

					var response = _mailService.SendMail(mailDto);

                    if (response is BadRequestObjectResult badRequest)
                    {
						Console.WriteLine("ERROR : " + badRequest.Value);
                    }

                }

				return;

            }
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}


		// To Update The Event IsCompleted
		public void UpdateEventStatus(int eventId)
		{
			try
			{

				var selectedServices = _context.SelectedServiceTable.Where(service => service.EventId == eventId).ToList();
				var completedSelectedServices = _context.SelectedServiceTable.Where(service => service.IsServiceCompleted == true & service.EventId == eventId).ToList();


				if (selectedServices.Count == completedSelectedServices.Count)
				{
					var currentEvent = _context.EventModelTable.Where(e => e.EventId == eventId).FirstOrDefault();

					currentEvent.IsCompleted = true;
					_context.SaveChanges();

					//AuctionModelTable newAuction = new AuctionModelTable();
					//newAuction.EventId = currentEvent.EventId;
					//newA
					return;
				}

				return;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}


		// Reschedule Auction
		public IActionResult RescheduleAuction(RescheduleAuctionDto rescheduleRequest)
		{
			try
			{
				var rescheduleEvent = _context.EventModelTable.Where(e => e.EventId == rescheduleRequest.EventId).FirstOrDefault();

				// NEED : Change the Auction Time in Production
				rescheduleEvent.AuctionTimeLimit = DateTime.Now.AddMinutes(2);

				rescheduleEvent.IsAuctionCompleted = false;

				_context.SaveChanges();

				// To notify vendor about the auction reschedule
				List<SelectedServiceModel> selectedServiceModels = _context.SelectedServiceTable.Where(s => s.SelectedServiceId == rescheduleRequest.SelectedServiceId).ToList();

				string subject = "A Auction is Rescheduled";

				SendMailForVendors(subject,rescheduleEvent, selectedServiceModels);


				return new OkObjectResult("Auction is Successfully Rescheduled");
				 


			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}

		}


	}
}
