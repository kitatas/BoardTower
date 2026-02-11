using System.Threading;
using BoardTower.Base.Data.Entity;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Base.Domain.UseCase
{
    public abstract class BasePubSubUseCase<T>
    {
        protected readonly BaseEntity<T> _entity;
        protected readonly IAsyncSubscriber<T> _subscriber;
        protected readonly IAsyncPublisher<T> _publisher;

        protected BasePubSubUseCase(BaseEntity<T> entity, IAsyncSubscriber<T> subscriber, IAsyncPublisher<T> publisher)
        {
            _entity = entity;
            _subscriber = subscriber;
            _publisher = publisher;
        }

        public virtual IAsyncSubscriber<T> subscriber => _subscriber;

        public virtual async UniTask PublishAsync(T value, CancellationToken token)
        {
            _entity.Set(value);
            await _publisher.PublishAsync(_entity.value, token);
        }
    }
}