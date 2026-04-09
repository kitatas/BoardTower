using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class HudRootUseCase
    {
        private readonly HudRootPorts _hudRootPorts;

        public HudRootUseCase(HudRootPorts hudRootPorts)
        {
            _hudRootPorts = hudRootPorts;
        }

        public IAsyncSubscriber<HudRootTransitionVO> transition => _hudRootPorts.hudRootTransitionSubscriber;
    }
}