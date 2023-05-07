using Biddo.Models;
using Biddo.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Biddo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly BiddoContext _context;
        private readonly IChatService _chatService;

        public ChatController(BiddoContext context, IChatService chatService)
        {
            _context = context;
            _chatService = chatService;
        }

        // To Get All Conversation of the Current user. 
        [HttpGet("get/conversations"), Authorize]
        public IEnumerable<object> GetConversations()
        {
            var response = _chatService.GetConversations();

            return response;
        }

        // GET Chats based on the conversationId
        [HttpGet("get/chats/{id}"), Authorize]
        public IEnumerable<TimelineCommentModel> GetChats(int id)
        {
            var response = _chatService.GetChats(id);

            return response;
        }

        // To GET chats based on the ticketId
        [HttpGet("get/chat/tickets/{ticketId}"), Authorize]
        public IEnumerable<TimelineCommentModel> GetChatsForTicket(int ticketId)
        {
            var response = this._chatService.GetChatForTickets(ticketId);

            return response;
        }

        // Post a message on the Conversation

        [HttpPost("send/message"), Authorize]
        public IActionResult SendMessage([FromBody] ChatDto chat)
        {
            var response = _chatService.AddChat(chat);

            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

        // Post a message on the Ticket

        [HttpPost("send/ticket/message"), Authorize]
        public IActionResult SendMessageForTicket([FromBody] ChatTicketDto chat)
        {
            var response = _chatService.AddChatForTicket(chat);

            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }
    }
}
