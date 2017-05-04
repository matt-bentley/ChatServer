using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatServer.Models
{
    /// <summary>
    /// A generic object to represent a broadcasted message
    /// </summary>
    public class MessageEvent
    {
        public MessageEvent()
        {
            Timestamp = DateTimeOffset.Now;
        }

        /// <summary>
        /// The name of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The name of the Chat Room
        /// </summary>
        public string ChatRoom { get; set; }

        /// <summary>
        /// The date/time that the message was sent
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// The chat message
        /// </summary>
        public string Message { get; set; }

    }
}