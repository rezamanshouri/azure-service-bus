namespace ServiceBusConsumer
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;

    public class ConsumerService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
