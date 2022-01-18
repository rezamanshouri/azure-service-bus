namespace ServiceBusConsumer.Consume
{
    using System.Threading;
    using System.Threading.Tasks;

    using Azure.Messaging.ServiceBus;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    using ServiceBusContracts;

    public class ConsumerService : BackgroundService
    {
        private readonly ServiceBusProcessor serviceBusProcessor;

        public ConsumerService(ServiceBusClient serviceBusClient, IConfiguration configuration)
        {
            var serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            this.serviceBusProcessor = serviceBusClient.CreateProcessor(
                configuration.GetValue<string>("ServiceBus:TopicName"),
                configuration.GetValue<string>("ServiceBus:SubscriptionName"),
                serviceBusProcessorOptions);
        }

        public override void Dispose()
        {
            if (serviceBusProcessor != null)
            {
                serviceBusProcessor.DisposeAsync().ConfigureAwait(false);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Configure processor
            serviceBusProcessor.ProcessMessageAsync += ProcessMessagesAsync;
            serviceBusProcessor.ProcessErrorAsync += ProcessErrorAsync;

            // Start receiving messages
            serviceBusProcessor.StartProcessingAsync();

            return Task.CompletedTask;
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            System.Console.WriteLine($"Message handler encountered an exception: {args.Exception}");

            return Task.CompletedTask;
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var order = args.Message.Body.ToObjectFromJson<Order>();

            // Do what you need with the message here
            System.Console.WriteLine($"New order: {order.Id} {order.Name}");

            await args.CompleteMessageAsync(args.Message);
        }
    }
}
