using System.Threading;
using BoardTower.Boot.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Boot.Domain.Ports
{
    public sealed class DisplayNamePorts
    {
        public readonly IAsyncSubscriber<DisplayNameTransitionVO> displayNameTransitionSubscriber;
        private readonly IAsyncPublisher<DisplayNameTransitionVO> _displayNameTransitionPublisher;

        public DisplayNamePorts(IAsyncSubscriber<DisplayNameTransitionVO> displayNameTransitionSubscriber,
            IAsyncPublisher<DisplayNameTransitionVO> displayNameTransitionPublisher)
        {
            this.displayNameTransitionSubscriber = displayNameTransitionSubscriber;
            _displayNameTransitionPublisher = displayNameTransitionPublisher;
        }

        public UniTask PublishDisplayNameTransitionAsync(DisplayNameTransitionVO displayNameTransition,
            CancellationToken token)
        {
            return _displayNameTransitionPublisher.PublishAsync(displayNameTransition, token);
        }
    }
}