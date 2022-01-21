namespace ServiceBusProducer.Tests.Publish
{
    using System.Collections.Generic;
    using System.Text.Json;

    using Azure.Messaging.ServiceBus;

    using Microsoft.Extensions.Configuration;

    using Moq;

    using ServiceBusContracts;

    using ServiceBusProducer.Services;

    using Xunit;

    public class MessagePublisherTests
    {
        private readonly MessagePublisher messagePublisher;
        private readonly Mock<ServiceBusClient> serviceBusClient;
        private readonly Mock<ServiceBusSender> serviceBusSender;

        public MessagePublisherTests()
        {
            const string TopicName = "topic name";
            var inMemorySettings = new Dictionary<string, string> { { "ServiceBus:TopicName", TopicName } };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            serviceBusClient = new Mock<ServiceBusClient>();
            serviceBusSender = new Mock<ServiceBusSender>();
            serviceBusClient.Setup(x => x.CreateSender(TopicName)).Returns(serviceBusSender.Object);

            messagePublisher = new MessagePublisher(serviceBusClient.Object, configuration);
        }

        [Fact]
        public void Publish_Success_MessageWasSent()
        {
            // Arrange
            var messageObject = new Order
            {
                Id = 10,
                Name = "my order"
            };

            // Act
            messagePublisher.Publish<Order>(messageObject);

            // Assert
            var expectedMessage = JsonSerializer.Serialize(messageObject);
            serviceBusSender.Verify(
                x => x.SendMessageAsync(
                    It.Is<ServiceBusMessage>(m => m.Body.ToString() == expectedMessage
                    && m.ApplicationProperties.Contains(new KeyValuePair<string, object>("messageType", "Order"))),
                    System.Threading.CancellationToken.None),
                Times.Once);
        }
    }
}
