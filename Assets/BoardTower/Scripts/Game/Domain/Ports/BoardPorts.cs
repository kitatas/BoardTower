using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class BoardPorts
    {
        public readonly IAsyncSubscriber<BoardTransitionVO> boardTransitionSubscriber;
        public readonly IAsyncSubscriber<RenderEventSquareVO> renderEventSquareSubscriber;
        private readonly IAsyncPublisher<BoardTransitionVO> _boardTransitionPublisher;
        private readonly IAsyncPublisher<RenderEventSquareVO> _renderEventSquarePublisher;

        public BoardPorts(IAsyncSubscriber<BoardTransitionVO> boardTransitionSubscriber,
            IAsyncSubscriber<RenderEventSquareVO> renderEventSquareSubscriber,
            IAsyncPublisher<BoardTransitionVO> boardTransitionPublisher,
            IAsyncPublisher<RenderEventSquareVO> renderEventSquarePublisher)
        {
            this.boardTransitionSubscriber = boardTransitionSubscriber;
            this.renderEventSquareSubscriber = renderEventSquareSubscriber;
            _boardTransitionPublisher = boardTransitionPublisher;
            _renderEventSquarePublisher = renderEventSquarePublisher;
        }

        public UniTask PublishBoardTransitionAsync(BoardTransitionVO boardTransition, CancellationToken token)
        {
            return _boardTransitionPublisher.PublishAsync(boardTransition, token);
        }

        public UniTask PublishEventSquaresAsync(RenderEventSquareVO renderEventSquare, CancellationToken token)
        {
            return _renderEventSquarePublisher.PublishAsync(renderEventSquare, token);
        }
    }
}