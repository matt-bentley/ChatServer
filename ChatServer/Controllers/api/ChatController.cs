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
using System.Web.Configuration;
using System.Web.Http;

namespace ChatServer.Controllers.api
{
    [RoutePrefix("api/chat")]
    public class ChatController : ApiController
    {
        private IHubContext _context;
        private static readonly string _adminId = WebConfigurationManager.AppSettings["adminKey"];

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

        [Route("AddRoom/{roomName}/{createdBy}")]
        [HttpPost]
        public IHttpActionResult AddRoom(string roomName, string createdBy)
        {
            ChatRoom chatRoom = new ChatRoom()
            {
                Name = roomName,
                CreatedBy = createdBy
            };

            ChatRoomHelper.TryAddChatRoom(chatRoom);

            _context.Clients.Group(_adminId).OnEvent(_adminId, new MessageEvent
            {
                Username = "Chat Server",
                ChatRoom = _adminId,
                Message = $"{roomName} created by {createdBy}"
            });

            return Ok(chatRoom);
        }

        [Route("PostMessage/{message}/{userName}/{chatRoom}")]
        [HttpPost]
        public IHttpActionResult PostMessage(string message, string userName, string chatRoom)
        {
            _context.Clients.Group(chatRoom).OnEvent(chatRoom, new MessageEvent
            {
                Username = userName,
                ChatRoom = chatRoom,
                Message = message
            });

            _context.Clients.Group(_adminId).OnEvent(_adminId, new MessageEvent
            {
                Username = userName,
                ChatRoom = chatRoom,
                Message = message
            });

            return Ok();
        }        
    }
}
