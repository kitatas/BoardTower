using System;
using System.Threading;
using BoardTower.Base.Data.Entity;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Base.Domain.UseCase
{
    public abstract class BaseStateUseCase<T> : BasePubSubUseCase<T> where T : Enum
    {
        protected BaseStateUseCase(BaseStateEntity<T> stateEntity, IAsyncSubscriber<T> subscriber,
            IAsyncPublisher<T> publisher) : base(stateEntity, subscriber, publisher)
        {
        }

        public abstract UniTask InitAsync(CancellationToken token);
    }
}