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

            // Application properties can be used to add filters at subscription level.
            // To do so, navigate to 'Correlation Filter' -> 'CUSTOM PROPERTIES'
            message.ApplicationProperties["messageType"] = typeof(T).Name;

            return serviceBusSender.SendMessageAsync(message);
        }

        public Task Publish(string rawMessage)
        {
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(rawMessage));
            message.ApplicationProperties["messageType"] = "RawMessage";
            return serviceBusSender.SendMessageAsync(message);
        }

        // Order of messages received from a topic subscription is not necessarily the same order by which they were sent to the topic.
        // Messages with the same session ID, however, will be received in the same order they were sent to the topic.
        // Note that:
        //   - The topic subscription must be session-enabled for this ordering to happen.
        //   - Once a topic subscription is session-enabled, messages without a session ID can't be received.
        public Task PublishWithSession<T>(T messageObject, string sessionId)
        {
            var jsonString = JsonSerializer.Serialize(messageObject);
            var message = new ServiceBusMessage(jsonString)
            {
                SessionId = sessionId
            };

            message.ApplicationProperties["messageType"] = typeof(T).Name;

            return serviceBusSender.SendMessageAsync(message);
        }
    }
}
