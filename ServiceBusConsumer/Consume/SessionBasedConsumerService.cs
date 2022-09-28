namespace ServiceBusConsumer.Consume
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Azure.Messaging.ServiceBus;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class SessionBasedConsumerService : BackgroundService
    {
        private readonly ServiceBusSessionProcessor serviceBusSessionProcessor;

        public SessionBasedConsumerService(ServiceBusClient serviceBusClient, IConfiguration configuration)
        {
            var options = new ServiceBusSessionProcessorOptions
            {
                ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete,
                AutoCompleteMessages = true,
                MaxConcurrentSessions = 1,
                MaxConcurrentCallsPerSession = 1,
            };

            this.serviceBusSessionProcessor = serviceBusClient.CreateSessionProcessor(
                configuration.GetValue<string>("ServiceBus:TopicName"),
                configuration.GetValue<string>("ServiceBus:SubscriptionName"),
                options);
        }

        public override void Dispose()
        {
            if (serviceBusSessionProcessor != null)
            {
                serviceBusSessionProcessor.DisposeAsync().ConfigureAwait(false);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // configure the message and error handler to use
            serviceBusSessionProcessor.ProcessMessageAsync += MessageHandler;
            serviceBusSessionProcessor.ProcessErrorAsync += ErrorHandler;

            async Task MessageHandler(ProcessSessionMessageEventArgs args)
            {
                var body = args.Message.Body.ToString();
                Console.WriteLine($"received body: {body}");
            }

            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                // the error source tells me at what point in the processing an error occurred
                Console.WriteLine(args.ErrorSource.ToString());
                // the fully qualified namespace is available
                Console.WriteLine(args.FullyQualifiedNamespace);
                // as well as the entity path
                Console.WriteLine(args.EntityPath);
                Console.WriteLine(args.Exception.ToString());
                return Task.CompletedTask;
            }

            // start processing
            serviceBusSessionProcessor.StartProcessingAsync();

            return Task.CompletedTask;
        }
    }
}
