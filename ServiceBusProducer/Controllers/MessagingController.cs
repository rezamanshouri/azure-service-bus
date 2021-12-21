namespace ServiceBusProducer.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("publish")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        [HttpGet("customer")]
        public IActionResult PublishCustomer()
        {
            return Ok();
        }

        [HttpGet("order")]
        public IActionResult PublishOrder()
        {
            return Ok();
        }
    }
}
