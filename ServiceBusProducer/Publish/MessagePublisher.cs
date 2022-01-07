namespace ServiceBusProducer.Services
{
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Azure.Messaging.ServiceBus;

    using Microsoft.Extensions.Configuration;

    using ServiceBusProducer.Publish;

    public class MessagePublisher : IMessagePublisher
    {
        private readonly ServiceBusSender serviceBusSender;

        public MessagePublisher(ServiceBusClient serviceBusClient, IConfiguration configuration)
        {
            serviceBusSender = serviceBusClient.CreateSender(configuration.GetValue<string>("ServiceBus:TopicName"));
        }

        public Task Publish<T>(T messageObject)
        {
            var jsonString = JsonSerializer.Serialize(messageObject);
            var message = new ServiceBusMessage(jsonString);
            message.ApplicationProperties["messageType"] = typeof(T).Name;
            return serviceBusSender.SendMessageAsync(message);
        }

        public Task Publish(string rawMessage)
        {
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(rawMessage));
            message.ApplicationProperties["messageType"] = "RawMessage";
            return serviceBusSender.SendMessageAsync(message);
        }
    }
}
