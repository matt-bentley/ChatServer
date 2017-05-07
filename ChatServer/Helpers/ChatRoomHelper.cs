using ChatServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace ChatServer.Helpers
{
    public static class ChatRoomHelper
    {
        public static void TryAddChatRoom(ChatRoom chatRoom)
        {
            ObjectCache cache = MemoryCache.Default;
            string tryRoom = cache[chatRoom.Id.ToString()] as string;
        
            if (tryRoom == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                cache.Set(chatRoom.Id.ToString(), chatRoom, policy);
            }
        }

        public static List<ChatRoom> GetChatRooms()
        {
            List<ChatRoom> chatRooms = new List<ChatRoom>();
            ObjectCache cache = MemoryCache.Default;
            foreach(var room in cache)
            {
                chatRooms.Add((ChatRoom)room.Value);
            }
            return chatRooms;
        }
    }
}