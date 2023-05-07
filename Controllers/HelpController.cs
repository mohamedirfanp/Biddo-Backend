using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biddo.Models;
using Microsoft.AspNetCore.Authorization;
using Biddo.Services.HelpServices;
using Biddo.Services.Models;

namespace Biddo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpController : ControllerBase
    {
        private readonly IHelpService _helpService;

        public HelpController(IHelpService helpService)
        {
            _helpService = helpService;
        }


        // To Get the Tickets
        [HttpGet("get/Tickets"), Authorize]
        public async Task<IEnumerable<QueryModel>> GetTickets()
        {
            var response = this._helpService.GetTickets();

            return response;
        }

        // To Create a Ticket
        [HttpPost("create/Ticket"), Authorize]
        public async Task<IActionResult> CreateTicket(TicketDto ticketDto)
        {
            var response = this._helpService.CreateTicket(ticketDto);

            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

        // To Get all the Ticket for Admin
        [HttpGet("get/all/Tickets"), Authorize(Roles = "Admin")]
        public async Task<IEnumerable<QueryModel>> GetAllTickets()
        {
            var response = this._helpService.GetAllTickets();

            return response;
        }

        // To Close The Ticket
        [HttpPut("close/Ticket"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> CloseTicket([FromBody] int tickedId)
        {
            var response = this._helpService.CloseTicket(tickedId);

            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }


    }
}
