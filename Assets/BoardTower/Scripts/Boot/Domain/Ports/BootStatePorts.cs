using BoardTower.Boot.Application;
using BoardTower.Common.Domain.Ports;
using MessagePipe;

namespace BoardTower.Boot.Domain.Ports
{
    public sealed class BootStatePorts : BaseStatePorts<BootState>
    {
        public BootStatePorts(IAsyncSubscriber<BootState> subscriber, IAsyncPublisher<BootState> publisher) : base(subscriber, publisher)
        {
        }
    }
}