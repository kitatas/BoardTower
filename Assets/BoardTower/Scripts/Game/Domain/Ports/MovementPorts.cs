using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class MovementPorts
    {
        public readonly IAsyncSubscriber<HighlightSquareVO[]> highlightsSubscriber;
        private readonly IAsyncPublisher<HighlightSquareVO[]> _highlightsPublisher;
        private readonly IAsyncPublisher<ChessmenMovementVO> _chessmenMovementPublisher;

        public MovementPorts(IAsyncSubscriber<HighlightSquareVO[]> highlightsSubscriber,
            IAsyncPublisher<HighlightSquareVO[]> highlightsPublisher,
            IAsyncPublisher<ChessmenMovementVO> chessmenMovementPublisher)
        {
            this.highlightsSubscriber = highlightsSubscriber;
            _highlightsPublisher = highlightsPublisher;
            _chessmenMovementPublisher = chessmenMovementPublisher;
        }

        public UniTask PublishHighlightSquaresAsync(HighlightSquareVO[] highlights, CancellationToken token)
        {
            return _highlightsPublisher.PublishAsync(highlights, token);
        }

        public UniTask PublishChessmenMovementAsync(ChessmenMovementVO movement, CancellationToken token)
        {
            return _chessmenMovementPublisher.PublishAsync(movement, token);
        }
    }
}