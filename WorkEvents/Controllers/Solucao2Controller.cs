using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WorkEvents.BackgroundServices;
using WorkEvents.Models;
using WorkEvents.SignalR;

namespace WorkEvents.Controllers
{
    public class Solucao2Controller : Controller
    {
        private readonly IHubContext<MyHub, IMyHub> _hubContext;
        private readonly IBackgroundTaskQueue _queue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        static List<Message> _messages = new List<Message>();

        public Solucao2Controller(
            IHubContext<MyHub, IMyHub> hubContext,
            IBackgroundTaskQueue queue,
            IServiceScopeFactory serviceScopeFactory)
        {
            _hubContext = hubContext;
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IActionResult Index()
        {
            return View(_messages);
        }

        [HttpPost]
        public async Task<IActionResult> DoEvent([FromBody] MessageCreateDto messageDto)
        {
            _queue.QueueBackgroundWorkItem(async token =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var message = await Message.CreateMessage(messageDto.Title, messageDto.Description);
                    _messages.Add(message);
                    await _hubContext.Clients.All.BroadcastMessageId(message.Id);
                }
            });

            return Ok();
        }

        [HttpGet("solucao2/message/{Id:Guid}")]
        public async Task<IActionResult> GetMessage(Guid Id)
        {
            var message = _messages.Where(m => m.Id.Equals(Id)).FirstOrDefault();
            return Ok(message);
        }
    }
}
