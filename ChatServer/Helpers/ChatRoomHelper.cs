using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace ChatServer.Helpers
{
    public static class ChatRoomHelper
    {
        public static void TryAddChatRoom(string chatRoom)
        {
            ObjectCache cache = MemoryCache.Default;
            string tryRoom = cache[chatRoom] as string;
        
            if (tryRoom == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                cache.Set(chatRoom, chatRoom, policy);
            }
        }

        public static List<string> GetChatRooms()
        {
            List<string> chatRooms = new List<string>();
            ObjectCache cache = MemoryCache.Default;
            foreach(var room in cache)
            {
                chatRooms.Add(room.Value.ToString());
            }
            return chatRooms;
        }
    }
}