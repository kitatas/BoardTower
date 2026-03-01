using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class BoardPorts
    {
        public readonly IAsyncSubscriber<BoardTransitionVO> boardTransitionSubscriber;
        public readonly IAsyncSubscriber<EventSquareVO[]> eventSquaresSubscriber;
        public readonly IAsyncPublisher<BoardTransitionVO> boardTransitionPublisher;
        public readonly IAsyncPublisher<EventSquareVO[]> eventSquaresPublisher;

        public BoardPorts(IAsyncSubscriber<BoardTransitionVO> boardTransitionSubscriber,
            IAsyncSubscriber<EventSquareVO[]> eventSquaresSubscriber,
            IAsyncPublisher<BoardTransitionVO> boardTransitionPublisher,
            IAsyncPublisher<EventSquareVO[]> eventSquaresPublisher)
        {
            this.boardTransitionSubscriber = boardTransitionSubscriber;
            this.eventSquaresSubscriber = eventSquaresSubscriber;
            this.boardTransitionPublisher = boardTransitionPublisher;
            this.eventSquaresPublisher = eventSquaresPublisher;
        }
    }
}