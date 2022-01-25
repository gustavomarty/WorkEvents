using WorkEvents.Models;

namespace WorkEvents.SignalR
{
    public interface IMyHub
    {
        Task BroadcastMessageId(Guid Id);
        Task BroadcastMessage(Message message);
    }
}
