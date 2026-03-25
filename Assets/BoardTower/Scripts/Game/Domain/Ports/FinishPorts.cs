using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class FinishPorts
    {
        public readonly IAsyncSubscriber<FinishVO> finishSubscriber;
        private readonly IAsyncPublisher<FinishVO> _finishPublisher;

        public FinishPorts(IAsyncSubscriber<FinishVO> finishSubscriber,
            IAsyncPublisher<FinishVO> finishPublisher)
        {
            this.finishSubscriber = finishSubscriber;
            _finishPublisher = finishPublisher;
        }

        public UniTask PublishFinishAsync(FinishVO finish, CancellationToken token)
        {
            return _finishPublisher.PublishAsync(finish, token);
        }
    }
}