namespace ServiceBusProducer.Publish
{
    using System.Threading.Tasks;

    public interface IMessagePublisher
    {
        Task Publish<T>(T messageObject);

        Task Publish(string rawMessage);

        Task PublishWithSession<T>(T messageObject, string sessionId);
    }
}
