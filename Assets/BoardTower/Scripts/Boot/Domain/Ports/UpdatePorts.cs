using System.Threading;
using BoardTower.Boot.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Boot.Domain.Ports
{
    public sealed class UpdatePorts
    {
        public readonly IAsyncSubscriber<UpdateTransitionVO> updateTransitionSubscriber;
        private readonly IAsyncPublisher<UpdateTransitionVO> _updateTransitionPublisher;

        public UpdatePorts(IAsyncSubscriber<UpdateTransitionVO> updateTransitionSubscriber,
            IAsyncPublisher<UpdateTransitionVO> updateTransitionPublisher)
        {
            this.updateTransitionSubscriber = updateTransitionSubscriber;
            _updateTransitionPublisher = updateTransitionPublisher;
        }

        public UniTask PublishUpdateTransitionAsync(UpdateTransitionVO updateTransitionVO, CancellationToken token)
        {
            return _updateTransitionPublisher.PublishAsync(updateTransitionVO, token);
        }
    }
}