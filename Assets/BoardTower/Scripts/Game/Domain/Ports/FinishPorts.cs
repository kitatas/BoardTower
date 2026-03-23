using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class FinishPorts
    {
        public readonly IAsyncSubscriber<FinishVO> finishSubscriber;
        public readonly IAsyncPublisher<FinishVO> finishPublisher;

        public FinishPorts(IAsyncSubscriber<FinishVO> finishSubscriber,
            IAsyncPublisher<FinishVO> finishPublisher)
        {
            this.finishSubscriber = finishSubscriber;
            this.finishPublisher = finishPublisher;
        }
    }
}