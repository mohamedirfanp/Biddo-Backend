using Biddo.Models;
using Biddo.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Biddo.Services.HelpServices
{
    public class HelpService : IHelpService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BiddoContext _context;
        private readonly IMailService _mailService;
        private readonly IHelpService _helpService;

        private readonly int adminId = 8;

        public HelpService(BiddoContext context, IHttpContextAccessor httpContextAccessor, IMailService mailService)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mailService = mailService;

        }

        // To Get the Current User ID
        public int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId);
        }


        // To Get the Tickets
        public IEnumerable<QueryModel> GetTickets()
        {
            try
            {

                var currentUserId = GetCurrentUserId();

                var allTickets = _context.QueryTable.Where(query => query.CreatedId ==  currentUserId).ToList();

                return allTickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<QueryModel>();


            }

        }

        // To Create the Ticket
        public IActionResult CreateTicket(TicketDto ticketDto)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var currentRole = this._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

                QueryModel newTicket = new QueryModel();
                newTicket.QueryTitle = ticketDto.TicketTitle;
                newTicket.QueryDesciption = ticketDto.TicketDescription;
                newTicket.QueryType = ticketDto.TicketType;

                newTicket.CreatedId = currentUserId;
                newTicket.CreatedAt = DateTime.Now;
                newTicket.Status = false;

                _context.QueryTable.Add(newTicket);
                _context.SaveChanges();

                TimelineCommentModel timelineCommentModel = new TimelineCommentModel();
                timelineCommentModel.QueryId = newTicket.QueryId;
                timelineCommentModel.message = $"Description : {ticketDto.TicketDescription}";
                timelineCommentModel.FromRole = currentRole;
                timelineCommentModel.From = currentUserId;
                timelineCommentModel.TimeStamp = DateTime.Now;

                _context.TimelineCommentModel.Add(timelineCommentModel);
                _context.SaveChanges();

                return new OkObjectResult("success");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }


        // To Get All Ticket for Admin
        public IEnumerable<QueryModel> GetAllTickets()
        {
            try
            {

                var currentUserId = GetCurrentUserId();

                var allTickets = _context.QueryTable.ToList();

                return allTickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<QueryModel>();


            }

        }

        // To Close a Ticket 
        public IActionResult CloseTicket(int ticketId)
        {
            try
            {
                var currentTicket = _context.QueryTable.Where(query => query.QueryId == ticketId).FirstOrDefault();

                currentTicket.Status = true;

                _context.SaveChanges();

                return new OkObjectResult("Success");

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }


    }
}
