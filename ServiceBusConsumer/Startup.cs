namespace ServiceBusConsumer
{
    using Microsoft.Azure.ServiceBus;
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
            services.AddSingleton<ISubscriptionClient>(x => new SubscriptionClient(
            Configuration.GetValue<string>("ServiceBus:ConnectionString"),
            Configuration.GetValue<string>("ServiceBus:TopicName"),
            Configuration.GetValue<string>("ServiceBus:SubscriptionName")));

            services.AddHostedService<ConsumerService>();
        }
    }
}
