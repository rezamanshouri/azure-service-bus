namespace ServiceBusProducer.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using ServiceBusContracts;

    using ServiceBusProducer.Publish;

    [Route("publish")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly IMessagePublisher messagePublisher;

        public MessagingController(IMessagePublisher messagePublisher)
        {
            this.messagePublisher = messagePublisher;
        }

        [HttpGet("publish-messages-with-session")]
        public IActionResult PublishMultipleMessagesWithSession()
        {
            var costomers = new List<Customer> {
                new Customer { FirstName = "1" },
                new Customer { FirstName = "2" },
                new Customer { FirstName = "3" },
                new Customer { FirstName = "4" },
                new Customer { FirstName = "5" },
             };

            foreach (var c in costomers)
            {
                this.messagePublisher.PublishWithSession(c, "some_session_ID");
            }

            return Ok();
        }

        [HttpPost("customer")]
        public IActionResult PublishCustomer([FromBody] Customer customer)
        {
            this.messagePublisher.Publish(customer);
            return Ok();
        }

        [HttpPost("order")]
        public IActionResult PublishOrder([FromBody] Order order)
        {
            this.messagePublisher.Publish(order);
            return Ok();
        }

        [HttpPost("text")]
        public IActionResult PublishRawMessage([FromBody] string rawMessage)
        {
            this.messagePublisher.Publish(rawMessage);
            return Ok();
        }
    }
}
