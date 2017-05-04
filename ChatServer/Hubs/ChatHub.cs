using ChatServer.Helpers;
using ChatServer.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Subscribe(string channel)
        {
            await Groups.Add(Context.ConnectionId, channel);

            var ev = new MessageEvent
            {
                ChatRoom = "admin",
                Username = "user.subscribed",
                Message = "subscribed to group"
            };

            ChatRoomHelper.TryAddChatRoom(channel);

            await Publish(ev);
        }

        public async Task Unsubscribe(string channel)
        {
            await Groups.Remove(Context.ConnectionId, channel);

            var ev = new MessageEvent
            {
                ChatRoom = "admin",
                Username = "user.unsubscribed",
                Message = "unsubscribed from group"
            };

            await Publish(ev);
        }


        public Task Publish(MessageEvent channelEvent)
        {
            Clients.Group(channelEvent.ChatRoom).OnEvent(channelEvent.ChatRoom, channelEvent);

            if (channelEvent.ChatRoom != "admin")
            {
                // Push this out on the admin channel
                //
                Clients.Group("admin").OnEvent("admin", channelEvent);
            }

            return Task.FromResult(0);
        }


        public override Task OnConnected()
        {
            var ev = new MessageEvent
            {
                ChatRoom = "admin",
                Username = "user.connected",
                Message = "user connected"
            };

            Publish(ev);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            var ev = new MessageEvent
            {
                ChatRoom = "admin",
                Username = "user.disconnected",
                Message = "user disconnected"
            };

            Publish(ev);

            return base.OnDisconnected(stopCalled);
        }
    }
}