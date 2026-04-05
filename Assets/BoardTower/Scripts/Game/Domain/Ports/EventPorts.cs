using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class EventPorts
    {
        private readonly IAsyncPublisher<ChessmenMovementVO> _movementPublisher;
        private readonly IAsyncPublisher<RenderEventSquareVO> _renderEventSquarePublisher;

        public EventPorts(IAsyncPublisher<ChessmenMovementVO> movementPublisher,
            IAsyncPublisher<RenderEventSquareVO> renderEventSquarePublisher)
        {
            _movementPublisher = movementPublisher;
            _renderEventSquarePublisher = renderEventSquarePublisher;
        }

        public UniTask PublishChessmenMovementAsync(ChessmenMovementVO movement, CancellationToken token)
        {
            return _movementPublisher.PublishAsync(movement, token);
        }

        public UniTask PublishEventSquaresAsync(RenderEventSquareVO renderEventSquare, CancellationToken token)
        {
            return _renderEventSquarePublisher.PublishAsync(renderEventSquare, token);
        }
    }
}