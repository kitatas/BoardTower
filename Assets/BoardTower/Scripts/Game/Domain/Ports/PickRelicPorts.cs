using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class PickRelicPorts
    {
        public readonly IAsyncSubscriber<PickRelicVO> pickRelicSubscriber;
        private readonly IAsyncPublisher<PickRelicVO> _pickRelicPublisher;

        public PickRelicPorts(IAsyncSubscriber<PickRelicVO> pickRelicSubscriber,
            IAsyncPublisher<PickRelicVO> pickRelicPublisher)
        {
            this.pickRelicSubscriber = pickRelicSubscriber;
            _pickRelicPublisher = pickRelicPublisher;
        }

        public UniTask PublishPickRelicAsync(PickRelicVO pickRelic, CancellationToken token)
        {
            return _pickRelicPublisher.PublishAsync(pickRelic, token);
        }
    }
}