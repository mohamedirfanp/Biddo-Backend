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

        public HelpService(BiddoContext context, IHttpContextAccessor httpContextAccessor, IMailService mailService, IHelpService helpService)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mailService = mailService;
            _helpService = helpService;

        }

        // To Get the Current User ID
        public int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId);
        }


        // To Get the Tickets
        public IEnumerable<QueryModel> GetAllTickets()
        {
            try
            {

                var currentUserId = GetCurrentUserId();

                var allTickets = _context.QueryTable.Where(query => query.UserId ==  currentUserId);

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

                QueryModel newTicket = new QueryModel();

                newTicket.QueryTitle = ticketDto.TicketTitle;
                newTicket.QueryDesciption = ticketDto.TicketDescription;
                newTicket.QueryType = ticketDto.TicketType;

                newTicket.CreatedAt = DateTime.Now;

                return new OkObjectResult("success");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}
