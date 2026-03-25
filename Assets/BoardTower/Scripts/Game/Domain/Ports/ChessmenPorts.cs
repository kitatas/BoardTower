using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class ChessmenPorts
    {
        public readonly IAsyncSubscriber<ChessmenTransitionVO> chessmenTransitionSubscriber;
        public readonly IAsyncSubscriber<ChessmenMovementVO> chessmenMovementSubscriber;
        private readonly IAsyncPublisher<ChessmenTransitionVO> _chessmenTransitionPublisher;

        public ChessmenPorts(IAsyncSubscriber<ChessmenTransitionVO> chessmenTransitionSubscriber,
            IAsyncSubscriber<ChessmenMovementVO> chessmenMovementSubscriber,
            IAsyncPublisher<ChessmenTransitionVO> chessmenTransitionPublisher)
        {
            this.chessmenTransitionSubscriber = chessmenTransitionSubscriber;
            this.chessmenMovementSubscriber = chessmenMovementSubscriber;
            _chessmenTransitionPublisher = chessmenTransitionPublisher;
        }

        public UniTask PublishChessmenTransitionAsync(ChessmenTransitionVO chessmenTransition, CancellationToken token)
        {
            return _chessmenTransitionPublisher.PublishAsync(chessmenTransition, token);
        }
    }
}