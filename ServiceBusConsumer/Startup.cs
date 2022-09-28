namespace ServiceBusConsumer
{
    using Azure.Messaging.ServiceBus;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using ServiceBusConsumer.Consume;

    public class Startup
    {
        IConfiguration Configuration
        {
            get;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ServiceBusClient>(x => new ServiceBusClient(Configuration.GetValue<string>("ServiceBus:ConnectionString")));

            //services.AddHostedService<ConsumerService>();
            services.AddHostedService<SessionBasedConsumerService>();
        }
    }
}
