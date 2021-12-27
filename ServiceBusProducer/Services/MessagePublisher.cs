namespace ServiceBusProducer.Services
{
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Azure.ServiceBus;

    public class MessagePublisher : IMessagePublisher
    {
        private readonly ITopicClient topicClient;

        public MessagePublisher(ITopicClient topicClient)
        {
            this.topicClient = topicClient;
        }

        public Task Publish<T>(T messageObject)
        {
            var jsonString = JsonSerializer.Serialize(messageObject);
            var message = new Message(Encoding.UTF8.GetBytes(jsonString));
            return topicClient.SendAsync(message);
        }

        public Task Publish(string rawMessage)
        {
            var message = new Message(Encoding.UTF8.GetBytes(rawMessage));
            return topicClient.SendAsync(message);
        }
    }
}
