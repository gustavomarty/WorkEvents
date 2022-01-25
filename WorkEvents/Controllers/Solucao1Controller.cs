using Microsoft.AspNetCore.Mvc;
using WorkEvents.BackgroundServices;
using WorkEvents.Models;

namespace WorkEvents.Controllers
{
    public class Solucao1Controller : Controller
    {
        private readonly IBackgroundTaskQueue _queue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        static List<Message> _messages = new List<Message>();

        public Solucao1Controller(
            IBackgroundTaskQueue queue,
            IServiceScopeFactory serviceScopeFactory)
        {
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
                }
            });

            return Ok();
        }
    }
}
