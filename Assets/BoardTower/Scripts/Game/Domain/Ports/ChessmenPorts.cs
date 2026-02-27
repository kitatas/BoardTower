using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class ChessmenPorts
    {
        public readonly IAsyncSubscriber<ChessmenTransitionVO> chessmenTransitionSubscriber;
        public readonly IAsyncPublisher<ChessmenTransitionVO> chessmenTransitionPublisher;

        public ChessmenPorts(IAsyncSubscriber<ChessmenTransitionVO> chessmenTransitionSubscriber,
            IAsyncPublisher<ChessmenTransitionVO> chessmenTransitionPublisher)
        {
            this.chessmenTransitionSubscriber = chessmenTransitionSubscriber;
            this.chessmenTransitionPublisher = chessmenTransitionPublisher;
        }
    }
}