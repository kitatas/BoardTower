using BoardTower.Base.Domain.Ports;
using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class GameStatePorts : BaseStatePorts<GameState>
    {
        public GameStatePorts(IAsyncSubscriber<GameState> subscriber, IAsyncPublisher<GameState> publisher) : base(
            subscriber, publisher)
        {
        }
    }
}