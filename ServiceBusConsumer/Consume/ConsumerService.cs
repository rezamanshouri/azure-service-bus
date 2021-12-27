namespace ServiceBusConsumer.Consume
{
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Hosting;

    using ServiceBusContracts;

    public class ConsumerService : BackgroundService
    {
        private readonly ISubscriptionClient subscriptionClient;

        public ConsumerService(ISubscriptionClient subscriptionClient)
        {
            this.subscriptionClient = subscriptionClient;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            subscriptionClient.RegisterMessageHandler((message, token) =>
            {
                var order = new Order();
                try
                {
                    order = JsonSerializer.Deserialize<Order>(Encoding.UTF8.GetString(message.Body));
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine("Exception was thrown deserializing message");
                }

                // Do what you need with the message here
                System.Console.WriteLine($"New order: {order.Id} {order.Name}");

                // 'message.SystemProperties.LockToken' tells the Service Bus to lock this message and don't send it to anyone else
                return subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }, new MessageHandlerOptions(args => Task.CompletedTask)
            {
                // This allows re-processing a message if processing one message failed
                AutoComplete = false,

                MaxConcurrentCalls = 1

                // You can also handle error here using 'args.Exception'
            });

            return Task.CompletedTask;
        }
    }
}
