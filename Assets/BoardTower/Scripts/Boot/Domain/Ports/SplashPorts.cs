using System.Threading;
using BoardTower.Boot.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Boot.Domain.Ports
{
    public sealed class SplashPorts
    {
        public readonly IAsyncSubscriber<SplashTransitionVO> splashTransitionSubscriber;
        private readonly IAsyncPublisher<SplashTransitionVO> _splashTransitionPublisher;

        public SplashPorts(IAsyncSubscriber<SplashTransitionVO> splashTransitionSubscriber,
            IAsyncPublisher<SplashTransitionVO> splashTransitionPublisher)
        {
            this.splashTransitionSubscriber = splashTransitionSubscriber;
            _splashTransitionPublisher = splashTransitionPublisher;
        }

        public UniTask PublishSplashTransitionAsync(SplashTransitionVO splashTransition, CancellationToken token)
        {
            return _splashTransitionPublisher.PublishAsync(splashTransition, token);
        }
    }
}