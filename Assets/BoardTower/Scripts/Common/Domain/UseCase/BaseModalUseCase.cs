using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Ports;
using Cysharp.Threading.Tasks;

namespace BoardTower.Common.Domain.UseCase
{
    public abstract class BaseModalUseCase<T> : BasePubSubUseCase<BaseModalTransitionVO<T>> where T : Enum
    {
        public BaseModalUseCase(BaseEntity<BaseModalTransitionVO<T>> entity,
            BasePubSubPorts<BaseModalTransitionVO<T>> ports) : base(entity, ports)
        {
        }

        protected abstract BaseModalTransitionVO<T> GetModalTransition(BaseModalVO<T> modal);

        public UniTask FadeAsync(BaseModalVO<T> modal, CancellationToken token)
        {
            return PublishAsync(GetModalTransition(modal), token);
        }
    }
}