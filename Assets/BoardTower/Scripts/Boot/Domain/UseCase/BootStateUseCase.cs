using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Data.Entity;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Common.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class BootStateUseCase : BaseStateUseCase<BootState>
    {
        public BootStateUseCase(BootStateEntity entity, BootStatePorts ports) : base(entity, ports)
        {
        }

        public override UniTask InitAsync(CancellationToken token)
        {
            return default;
        }

        public override UniTask PublishAsync(BootState value, CancellationToken token)
        {
            if (value is BootState.None) return UniTask.Yield(token);
            return base.PublishAsync(value, token);
        }
    }
}