using ChatServer.Helpers;
using ChatServer.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly string _adminId = WebConfigurationManager.AppSettings["adminKey"];

        public async Task Subscribe(string channel)
        {
            await Groups.Add(Context.ConnectionId, channel);

            var ev = new MessageEvent
            {
                ChatRoom = _adminId,
                Username = "Chat Server",
                Message = $"{Context.ConnectionId} subscribed to room {channel}"
            };

            await Publish(ev);
        }

        public async Task Unsubscribe(string channel)
        {
            await Groups.Remove(Context.ConnectionId, channel);

            var ev = new MessageEvent
            {
                ChatRoom = _adminId,
                Username = "Chat Server",
                Message = $"{Context.ConnectionId} unsubscribed to room {channel}"
            };

            await Publish(ev);
        }


        public Task Publish(MessageEvent messageEvent)
        {
            Clients.Group(messageEvent.ChatRoom).OnEvent(messageEvent.ChatRoom, messageEvent);

            if (messageEvent.ChatRoom != _adminId)
            {
                // Push this out on the admin channel
                //
                Clients.Group(_adminId).OnEvent(_adminId, messageEvent);
            }

            return Task.FromResult(0);
        }


        public override Task OnConnected()
        {
            var ev = new MessageEvent
            {
                ChatRoom = _adminId,
                Username = "Chat Server",
                Message = $"{Context.ConnectionId} connected to a hub"
            };

            Publish(ev);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            var ev = new MessageEvent
            {
                ChatRoom = _adminId,
                Username = "Chat Server",
                Message = $"{Context.ConnectionId} disconnected from a hub"
            };

            Publish(ev);

            return base.OnDisconnected(stopCalled);
        }
    }
}