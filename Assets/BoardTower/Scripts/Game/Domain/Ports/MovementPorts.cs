using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class MovementPorts
    {
        public readonly IAsyncSubscriber<HighlightSquareVO[]> highlightsSubscriber;
        public readonly IAsyncPublisher<HighlightSquareVO[]> highlightsPublisher;
        public readonly IAsyncPublisher<ChessmenMovementVO> chessmenMovementPublisher;

        public MovementPorts(IAsyncSubscriber<HighlightSquareVO[]> highlightsSubscriber,
            IAsyncPublisher<HighlightSquareVO[]> highlightsPublisher,
            IAsyncPublisher<ChessmenMovementVO> chessmenMovementPublisher)
        {
            this.highlightsSubscriber = highlightsSubscriber;
            this.highlightsPublisher = highlightsPublisher;
            this.chessmenMovementPublisher = chessmenMovementPublisher;
        }
    }
}