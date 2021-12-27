namespace ServiceBusProducer
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;

    using ServiceBusConsumer;

    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                   .ConfigureServices((hostBuilderContext, serviceCollection) => new Startup(hostBuilderContext.Configuration).ConfigureServices(serviceCollection));
    }
}
