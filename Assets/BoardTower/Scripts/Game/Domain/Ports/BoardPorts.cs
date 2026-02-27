using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class BoardPorts
    {
        public readonly IAsyncSubscriber<BoardTransitionVO> boardTransitionSubscriber;
        public readonly IAsyncPublisher<BoardTransitionVO> boardTransitionPublisher;

        public BoardPorts(IAsyncSubscriber<BoardTransitionVO> boardTransitionSubscriber,
            IAsyncPublisher<BoardTransitionVO> boardTransitionPublisher)
        {
            this.boardTransitionSubscriber = boardTransitionSubscriber;
            this.boardTransitionPublisher = boardTransitionPublisher;
        }
    }
}