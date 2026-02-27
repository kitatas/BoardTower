using MessagePipe;

namespace BoardTower.Base.Domain.Ports
{
    public abstract class BasePubSubPorts<T>
    {
        public readonly IAsyncSubscriber<T> subscriber;
        public readonly IAsyncPublisher<T> publisher;

        protected BasePubSubPorts(IAsyncSubscriber<T> subscriber, IAsyncPublisher<T> publisher)
        {
            this.subscriber = subscriber;
            this.publisher = publisher;
        }
    }
}