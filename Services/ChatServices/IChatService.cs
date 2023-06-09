﻿using Biddo.Models;
using Biddo.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biddo.Services.ChatServices
{
    public interface IChatService
    {

        // To Get the Conversation 
        IEnumerable<object> GetConversations();

        // To Get The TimeLine Comments
        IEnumerable<TimelineCommentModel> GetChats(int conversationId);

        // To Create Conversation
        IActionResult CreateConversation(ConversationModel conversation);

        // To Add Chat for a Conversation
        IActionResult AddChat(ChatDto chat);

        //To Get the Timeline Comment for Tickets
        IEnumerable<TimelineCommentModel> GetChatForTickets(int ticketId);

        // To Add Chat for a Ticket
        IActionResult AddChatForTicket(ChatTicketDto chatTicket);



    }
}
