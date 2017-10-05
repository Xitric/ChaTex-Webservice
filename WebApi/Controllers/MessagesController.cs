using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        //private readonly IMailCenter mailCenter;
        private readonly IMailCenter mailCenter;
        private readonly MessageAwaitQueue awaitQueue;

        public MessagesController(IMailCenter mailCenter, MessageAwaitQueue awaitQueue)
        {
            this.mailCenter = mailCenter;
            this.awaitQueue = awaitQueue;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(mailCenter.GetMessages());
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public IActionResult Get(long id)
        {
            if (id < 0)
            {
                return BadRequest("Message ID's are positive");
            }

            Message message = mailCenter.GetMessage(id);

            if (message == null)
            {
                return NotFound();
            }

            return new ObjectResult(message);
        }

        [HttpGet("wait")]
        public IActionResult Get(DateTime since)
        {
            if (since == null)
            {
                return BadRequest();
            }

            IEnumerable<Message> messages = mailCenter.GetMessages(since);
            while (!messages.Any())
            {
                awaitQueue.Await(); //The test for messages and the call to await should happen atomically! I'm lazy...
                messages = mailCenter.GetMessages(since);
            }

            return new ObjectResult(messages);
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] Message message)
        {
            if (message == null)
            {
                return BadRequest();
            }

            mailCenter.RegisterMessage(message);
            awaitQueue.Notify();
            return CreatedAtRoute("GetMessage", new { id = message.Id }, message);
        }
    }
}
