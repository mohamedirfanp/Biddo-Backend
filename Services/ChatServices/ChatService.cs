using Biddo.Models;
using Biddo.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Biddo.Services.ChatServices
{
    public class ChatService : IChatService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BiddoContext _context;
        private readonly IConfiguration _configuration;

        public ChatService(BiddoContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _configuration = configuration;
        }

        // To Get the Current User ID
        public int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId);
        }

        // To Get the Conversations between Members
        public IEnumerable<object> GetConversations()
        {
            try
            {
                var requestId = GetCurrentUserId();

                var conversationsList = _context.ConvensationTable.Include(conv => conv.User).Include(conv => conv.Vendor).Where(conv => conv.UserId == requestId || conv.VendorId == requestId).Select(conv => new
                {
                    conv.ConversationId,
                    conv.UserId,
                    conv.VendorId,
                    conv.User.Name,
                    conv.User.Email,
                    conv.Vendor.VendorName,
                    conv.Vendor.VendorEmail
                }).ToList();
                return conversationsList;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Enumerable.Empty<object>();
            }
        }

        // To get the Conversation Messages
        public IEnumerable<TimelineCommentModel> GetChats(int conversionId)
        {
            try
            {
                var chatsList = _context.TimelineCommentModel.Where(conv => conv.ConversationId == conversionId).ToList();

                return chatsList;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Enumerable.Empty<TimelineCommentModel>();
            }
        }


        // To Create a Conversation

        public IActionResult CreateConversation(ConversationModel newConversation)
        {
            try
            {
                _context.ConvensationTable.Add(newConversation);
                _context.SaveChanges();

                return new OkObjectResult("Success");
            }
            catch( Exception ex )
            {
                Console.WriteLine(ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // To Add a Message
        public IActionResult AddChat(ChatDto chat)
        {
            try
            {

                var currentId = GetCurrentUserId();
                var currentRole = this._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

                TimelineCommentModel newChat = new TimelineCommentModel();

                newChat.From = currentId;
                newChat.message = chat.Message;
                newChat.ConversationId = chat.ConversationId;
                newChat.FromRole = currentRole;
                newChat.TimeStamp = DateTime.Now;

                // TODO : Need to check for Permission to chat
                
                _context.TimelineCommentModel.Add(newChat);
                _context.SaveChanges();

                return new OkObjectResult("Success");

            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }



    }
}
