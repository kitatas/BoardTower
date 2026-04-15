using System;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Ports;

namespace BoardTower.Common.Domain.UseCase
{
    public abstract class BaseModalUseCase<T> : BasePubSubUseCase<BaseModalTransitionVO<T>> where T : Enum
    {
        public BaseModalUseCase(BaseEntity<BaseModalTransitionVO<T>> entity,
            BasePubSubPorts<BaseModalTransitionVO<T>> ports) : base(entity, ports)
        {
        }
    }
}