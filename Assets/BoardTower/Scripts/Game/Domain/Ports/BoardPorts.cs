using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class BoardPorts
    {
        public readonly IAsyncSubscriber<BoardTransitionVO> boardTransitionSubscriber;
        public readonly IAsyncSubscriber<EventSquareVO[]> eventSquaresSubscriber;
        private readonly IAsyncPublisher<BoardTransitionVO> _boardTransitionPublisher;
        private readonly IAsyncPublisher<EventSquareVO[]> _eventSquaresPublisher;

        public BoardPorts(IAsyncSubscriber<BoardTransitionVO> boardTransitionSubscriber,
            IAsyncSubscriber<EventSquareVO[]> eventSquaresSubscriber,
            IAsyncPublisher<BoardTransitionVO> boardTransitionPublisher,
            IAsyncPublisher<EventSquareVO[]> eventSquaresPublisher)
        {
            this.boardTransitionSubscriber = boardTransitionSubscriber;
            this.eventSquaresSubscriber = eventSquaresSubscriber;
            _boardTransitionPublisher = boardTransitionPublisher;
            _eventSquaresPublisher = eventSquaresPublisher;
        }

        public UniTask PublishBoardTransitionAsync(BoardTransitionVO boardTransition, CancellationToken token)
        {
            return _boardTransitionPublisher.PublishAsync(boardTransition, token);
        }

        public UniTask PublishEventSquaresAsync(EventSquareVO[] eventSquares, CancellationToken token)
        {
            return _eventSquaresPublisher.PublishAsync(eventSquares, token);
        }
    }
}