using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WorkEvents.Models;
using WorkEvents.ServiceBus;
using WorkEvents.SignalR;

namespace WorkEvents.Controllers
{
    public class Solucao3Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoEvent([FromBody] MessageCreateDto messageDto)
        {
            await ServiceBusService.SendMessage(JsonConvert.SerializeObject(messageDto));
            return Ok();
        }
    }
}
