using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class EventPorts
    {
        private readonly IAsyncPublisher<ChessmenMovementVO> _movementPublisher;
        private readonly IAsyncPublisher<EventSquareVO[]> _eventSquaresPublisher;

        public EventPorts(IAsyncPublisher<ChessmenMovementVO> movementPublisher,
            IAsyncPublisher<EventSquareVO[]> eventSquaresPublisher)
        {
            _movementPublisher = movementPublisher;
            _eventSquaresPublisher = eventSquaresPublisher;
        }

        public UniTask PublishChessmenMovementAsync(ChessmenMovementVO movement, CancellationToken token)
        {
            return _movementPublisher.PublishAsync(movement, token);
        }

        public UniTask PublishEventSquaresAsync(EventSquareVO[] eventSquares, CancellationToken token)
        {
            return _eventSquaresPublisher.PublishAsync(eventSquares, token);
        }
    }
}