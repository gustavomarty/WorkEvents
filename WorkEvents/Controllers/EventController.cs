using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WorkEvents.ServiceBus;
using Microsoft.AspNetCore.SignalR;
using WorkEvents.SignalR;
using WorkEvents.BackgroundServices;
using WorkEvents.Models;

namespace WorkEvents.Controllers
{
    public class EventController : Controller
    {
        private readonly IHubContext<MyHub, IMyHub> _hubContext;
        private readonly IBackgroundTaskQueue _queue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventController(
            IHubContext<MyHub, IMyHub> hubContext,
            IBackgroundTaskQueue queue,
            IServiceScopeFactory serviceScopeFactory)
        {
            _hubContext = hubContext;
            _queue = queue; 
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AsyncEvent([FromBody] Message message)
        {
            await ServiceBusService.SendMessage(message.Description);

            _queue.QueueBackgroundWorkItem(async token =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await Task.Delay(5000);
                    await _hubContext.Clients.All.BroadcastMessage(message);
                }
            });

            return Ok();
        }


    }
}
