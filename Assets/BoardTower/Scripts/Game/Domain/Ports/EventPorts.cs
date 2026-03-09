using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class EventPorts
    {
        public readonly IAsyncPublisher<ChessmenMovementVO> movementPublisher;
        public readonly IAsyncPublisher<EventSquareVO[]> eventSquaresPublisher;

        public EventPorts(IAsyncPublisher<ChessmenMovementVO> movementPublisher,
            IAsyncPublisher<EventSquareVO[]> eventSquaresPublisher)
        {
            this.movementPublisher = movementPublisher;
            this.eventSquaresPublisher = eventSquaresPublisher;
        }
    }
}