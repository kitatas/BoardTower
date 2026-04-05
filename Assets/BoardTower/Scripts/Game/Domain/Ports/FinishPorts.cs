using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class FinishPorts
    {
        public readonly IAsyncSubscriber<FinishTransitionVO> finishTransitionSubscriber;
        private readonly IAsyncPublisher<FinishTransitionVO> _finishTransitionPublisher;

        public FinishPorts(IAsyncSubscriber<FinishTransitionVO> finishTransitionSubscriber,
            IAsyncPublisher<FinishTransitionVO> finishTransitionPublisher)
        {
            this.finishTransitionSubscriber = finishTransitionSubscriber;
            _finishTransitionPublisher = finishTransitionPublisher;
        }

        public UniTask PublishFinishAsync(FinishTransitionVO finishTransition, CancellationToken token)
        {
            return _finishTransitionPublisher.PublishAsync(finishTransition, token);
        }
    }
}