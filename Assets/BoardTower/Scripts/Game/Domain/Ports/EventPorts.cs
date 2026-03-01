using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class EventPorts
    {
        public readonly IAsyncPublisher<ChessmenMovementVO> movementPublisher;

        public EventPorts(IAsyncPublisher<ChessmenMovementVO> movementPublisher)
        {
            this.movementPublisher = movementPublisher;
        }
    }
}