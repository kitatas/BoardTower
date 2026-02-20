using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class BoardUseCase
    {
        private readonly IAsyncSubscriber<BoardTransitionVO> _subscriber;
        private readonly IAsyncPublisher<BoardTransitionVO> _publisher;

        public BoardUseCase(IAsyncSubscriber<BoardTransitionVO> subscriber,
            IAsyncPublisher<BoardTransitionVO> publisher)
        {
            _subscriber = subscriber;
            _publisher = publisher;
        }

        public IAsyncSubscriber<BoardTransitionVO> subscriber => _subscriber;

        public async UniTask InitAsync(CancellationToken token)
        {
            await _publisher.PublishAsync(new BoardTransitionVO(Fade.Out), token);
        }

        public async UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            await _publisher.PublishAsync(new BoardTransitionVO(fade, BoardConfig.FADE_DURATION), token);
        }
    }
}