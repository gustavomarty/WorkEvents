using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WorkEvents.Models;

namespace WorkEvents.SignalR
{
    public class MyHub : Hub<IMyHub>
    {
        public Task BroadcastMessageId(Guid Id) =>
            Clients.All.BroadcastMessageId(Id);

        public Task BroadcastMessage(Message message) =>
            Clients.All.BroadcastMessage(message);
        
    }
}