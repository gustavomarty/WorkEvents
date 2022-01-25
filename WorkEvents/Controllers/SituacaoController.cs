using Microsoft.AspNetCore.Mvc;
using WorkEvents.Models;

namespace WorkEvents.Controllers
{
    public class SituacaoController : Controller
    {
        static List<Message> _messages = new List<Message>();

        public IActionResult Index()
        {
            return View(_messages);
        }

        [HttpPost]
        public async Task<IActionResult> DoEvent([FromBody] MessageCreateDto messageDto)
        {
            var message = await Message.CreateMessage(messageDto.Title, messageDto.Description);
            _messages.Add(message);
            return Ok(message);
        }
    }
}
