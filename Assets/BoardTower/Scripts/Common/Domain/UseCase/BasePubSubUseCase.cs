using System.Threading;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Common.Domain.UseCase
{
    public abstract class BasePubSubUseCase<T>
    {
        protected readonly BaseEntity<T> _entity;
        protected readonly BasePubSubPorts<T> _ports;

        protected BasePubSubUseCase(BaseEntity<T> entity, BasePubSubPorts<T> ports)
        {
            _entity = entity;
            _ports = ports;
        }

        public virtual IAsyncSubscriber<T> subscriber => _ports.subscriber;

        public virtual UniTask PublishAsync(T value, CancellationToken token)
        {
            _entity.Set(value);
            return _ports.PublishAsync(_entity.value, token);
        }
    }
}