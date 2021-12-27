namespace ServiceBusProducer.Controllers
{
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
