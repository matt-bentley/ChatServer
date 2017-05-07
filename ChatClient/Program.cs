using ChatServer.Models;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:56472/";
            Console.ReadLine();
            var hubConnection = new HubConnection(baseAddress);
            IHubProxy eventHubProxy = hubConnection.CreateHubProxy("ChatHub");
            eventHubProxy.On<string, MessageEvent>("OnEvent", (channel, ev) =>
            {
                Console.WriteLine($"Event received on {channel} channel - {ev.Message}");
            });
            hubConnection.Start().Wait();

            // Join the channel for task updates in our console window
            //
            eventHubProxy.Invoke("Subscribe", "e7de7c74-14a4-4541-bd95-512a2717e7ab");
            eventHubProxy.Invoke("Subscribe", "demoRoom");

            Console.WriteLine($"Server is running on {baseAddress}");
            Console.WriteLine("Press <enter> to stop server");
            Console.ReadLine();
        }
    }
}
