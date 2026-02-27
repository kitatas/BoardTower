using System;
using System.Threading;
using BoardTower.Base.Data.Entity;
using BoardTower.Base.Domain.Ports;
using Cysharp.Threading.Tasks;

namespace BoardTower.Base.Domain.UseCase
{
    public abstract class BaseStateUseCase<T> : BasePubSubUseCase<T> where T : Enum
    {
        protected BaseStateUseCase(BaseEntity<T> entity, BaseStatePorts<T> ports) : base(entity, ports)
        {
        }

        public abstract UniTask InitAsync(CancellationToken token);
    }
}