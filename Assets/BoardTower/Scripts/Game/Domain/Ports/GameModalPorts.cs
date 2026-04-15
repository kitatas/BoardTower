using BoardTower.Common.Application;
using BoardTower.Common.Domain.Ports;
using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class GameModalPorts : BasePubSubPorts<BaseModalTransitionVO<GameModalType>>
    {
        public GameModalPorts(IAsyncSubscriber<BaseModalTransitionVO<GameModalType>> subscriber,
            IAsyncPublisher<BaseModalTransitionVO<GameModalType>> publisher) : base(subscriber, publisher)
        {
        }
    }
}