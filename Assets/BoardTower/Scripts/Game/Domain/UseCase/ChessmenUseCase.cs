using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class ChessmenUseCase
    {
        private readonly IAsyncSubscriber<ChessmenTransitionVO> _subscriber;
        private readonly IAsyncPublisher<ChessmenTransitionVO> _publisher;

        public ChessmenUseCase(IAsyncSubscriber<ChessmenTransitionVO> subscriber,
            IAsyncPublisher<ChessmenTransitionVO> publisher)
        {
            _subscriber = subscriber;
            _publisher = publisher;
        }

        public IAsyncSubscriber<ChessmenTransitionVO> subscriber => _subscriber;
    }
}