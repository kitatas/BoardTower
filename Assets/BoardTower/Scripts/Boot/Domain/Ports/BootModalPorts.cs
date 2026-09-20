using BoardTower.Boot.Application;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Ports;
using MessagePipe;

namespace BoardTower.Boot.Domain.Ports
{
    public sealed class BootModalPorts : BasePubSubPorts<BaseModalTransitionVO<BootModalType>>
    {
        public BootModalPorts(IAsyncSubscriber<BaseModalTransitionVO<BootModalType>> subscriber,
            IAsyncPublisher<BaseModalTransitionVO<BootModalType>> publisher) : base(subscriber, publisher)
        {
        }
    }
}