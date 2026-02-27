using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class BoardUseCase
    {
        private readonly BoardPorts _boardPorts;

        public BoardUseCase(BoardPorts boardPorts)
        {
            _boardPorts = boardPorts;
        }

        public IAsyncSubscriber<BoardTransitionVO> transition => _boardPorts.boardTransitionSubscriber;

        public async UniTask InitAsync(CancellationToken token)
        {
            await _boardPorts.boardTransitionPublisher
                .PublishAsync(new BoardTransitionVO(Fade.Out), token);
        }

        public async UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            await _boardPorts.boardTransitionPublisher
                .PublishAsync(new BoardTransitionVO(fade, BoardConfig.FADE_DURATION), token);
        }
    }
}