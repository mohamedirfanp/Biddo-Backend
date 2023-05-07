using Biddo.Models;
using Biddo.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biddo.Services.HelpServices
{
    public interface IHelpService
    {
        // To Get Ticket for current User
        IEnumerable<QueryModel> GetTickets();

        // To Create Ticket for current User
        IActionResult CreateTicket(TicketDto ticketDto);

        // To Get All the Ticket for Admin
        IEnumerable<QueryModel> GetAllTickets();

        // To Close a Ticket
        IActionResult CloseTicket(int  ticketId);

    }
}
