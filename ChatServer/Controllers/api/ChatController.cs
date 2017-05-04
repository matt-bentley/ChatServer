using ChatServer.Helpers;
using ChatServer.Hubs;
using ChatServer.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace ChatServer.Controllers.api
{
    [RoutePrefix("api/chat")]
    public class ChatController : ApiController
    {
        private IHubContext _context;

        private string _channel = "tasks";

        public ChatController()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        }

        [Route("GetRooms")]
        [HttpGet]
        public IHttpActionResult GetRooms()
        {
            var chatRooms = ChatRoomHelper.GetChatRooms();

            return Ok(chatRooms);
        }

        [Route("PostMessage/{message}")]
        [HttpPost]
        public IHttpActionResult PostMessage(string message)
        {
            _context.Clients.Group(_channel).OnEvent("tasks", new MessageEvent
            {
                Username = "api",
                ChatRoom = "demo",
                Message = message
            });

            return Ok();
        }        
    }
}
