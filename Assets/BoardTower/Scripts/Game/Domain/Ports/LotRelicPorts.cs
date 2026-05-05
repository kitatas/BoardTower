using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class LotRelicPorts
    {
        public readonly IAsyncSubscriber<LotRelicTransitionVO> lotRelicTransitionSubscriber;
        private readonly IAsyncPublisher<LotRelicTransitionVO> _lotRelicTransitionPublisher;

        public LotRelicPorts(IAsyncSubscriber<LotRelicTransitionVO> lotRelicTransitionSubscriber,
            IAsyncPublisher<LotRelicTransitionVO> lotRelicTransitionPublisher)
        {
            this.lotRelicTransitionSubscriber = lotRelicTransitionSubscriber;
            _lotRelicTransitionPublisher = lotRelicTransitionPublisher;
        }

        public UniTask PublishLotRelicTransitionAsync(LotRelicTransitionVO lotRelicTransition, CancellationToken token)
        {
            return _lotRelicTransitionPublisher.PublishAsync(lotRelicTransition, token);
        }
    }
}