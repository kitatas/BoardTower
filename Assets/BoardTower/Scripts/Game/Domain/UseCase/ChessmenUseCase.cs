using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class ChessmenUseCase
    {
        private readonly ChessmenEntity _chessmenEntity;
        private readonly IAsyncSubscriber<ChessmenTransitionVO> _subscriber;
        private readonly IAsyncPublisher<ChessmenTransitionVO> _publisher;

        public ChessmenUseCase(ChessmenEntity chessmenEntity, IAsyncSubscriber<ChessmenTransitionVO> subscriber,
            IAsyncPublisher<ChessmenTransitionVO> publisher)
        {
            _chessmenEntity = chessmenEntity;
            _subscriber = subscriber;
            _publisher = publisher;
        }

        public IAsyncSubscriber<ChessmenTransitionVO> subscriber => _subscriber;

        public async UniTask InitAsync(CancellationToken token)
        {
            _chessmenEntity.Init();
            await _publisher.PublishAsync(new ChessmenTransitionVO(Fade.Out), token);
        }

        public async UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            await _publisher.PublishAsync(new ChessmenTransitionVO(fade, ChessmenConfig.FADE_DURATION), token);
        }
    }
}