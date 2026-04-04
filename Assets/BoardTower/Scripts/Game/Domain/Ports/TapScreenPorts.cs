using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class TapScreenPorts
    {
        public readonly IAsyncSubscriber<TapScreenVO> tapScreenSubscriber;
        private readonly IAsyncPublisher<TapScreenVO> _tapScreenPublisher;

        public TapScreenPorts(IAsyncSubscriber<TapScreenVO> tapScreenSubscriber,
            IAsyncPublisher<TapScreenVO> tapScreenPublisher)
        {
            this.tapScreenSubscriber = tapScreenSubscriber;
            _tapScreenPublisher = tapScreenPublisher;
        }

        public UniTask PublishTapScreenAsync(TapScreenVO tapScreen, CancellationToken token)
        {
            return _tapScreenPublisher.PublishAsync(tapScreen, token);
        }
    }
}