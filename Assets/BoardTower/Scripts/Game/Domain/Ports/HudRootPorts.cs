using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class HudRootPorts
    {
        public readonly IAsyncSubscriber<HudRootTransitionVO> hudRootTransitionSubscriber;
        private readonly IAsyncPublisher<HudRootTransitionVO> _hudRootTransitionPublisher;

        public HudRootPorts(IAsyncSubscriber<HudRootTransitionVO> hudRootTransitionSubscriber,
            IAsyncPublisher<HudRootTransitionVO> hudRootTransitionPublisher)
        {
            this.hudRootTransitionSubscriber = hudRootTransitionSubscriber;
            _hudRootTransitionPublisher = hudRootTransitionPublisher;
        }

        public UniTask PublishHudRootTransitionAsync(HudRootTransitionVO parameterTransition, CancellationToken token)
        {
            return _hudRootTransitionPublisher.PublishAsync(parameterTransition, token);
        }
    }
}