using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
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

        public async UniTask InitAsync(CancellationToken token)
        {
            await _publisher.PublishAsync(new ChessmenTransitionVO(Fade.Out), token);
        }

        public async UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            await _publisher.PublishAsync(new ChessmenTransitionVO(fade, ChessmenConfig.FADE_DURATION), token);
        }
    }
}