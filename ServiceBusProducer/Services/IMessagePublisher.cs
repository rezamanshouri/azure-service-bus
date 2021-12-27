namespace ServiceBusProducer.Services
{
    using System.Threading.Tasks;

    public interface IMessagePublisher
    {
        Task Publish<T>(T messageObject);

        Task Publish(string rawMessage);
    }
}
