using isRock.LineBot;
using LineBot_api.Filters;
using LineBot_api.Models;
using LineBot_api.Service.Interface;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LineBot_api.Controllers
{
    [Produces("application/json")]
    [ServiceFilter(typeof(ReturnFormatFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController(IMessageService _messageService): ControllerBase
    {
        // GET: api/<MessageController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_messageService.seed("seed mygo"));
        }
        // POST api/<MessageController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}
        [HttpPost("Webhook")]
        public IActionResult Webhook(WebhookRequestBodyDto body)
        {
            return Ok(_messageService.ReceiveWebhook(body));
        }
        [HttpPost("SendMessage/Broadcast")]
        public IActionResult Broadcast([Required] string messageType, object body)
        {
            return Ok(_messageService.BroadcastMessageHandler(messageType, body));
        }
    }
}
