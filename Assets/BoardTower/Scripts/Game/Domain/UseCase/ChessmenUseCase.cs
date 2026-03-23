using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class ChessmenUseCase
    {
        private readonly ChessmenEntity _chessmenEntity;
        private readonly ChessmenPorts _chessmenPorts;

        public ChessmenUseCase(ChessmenEntity chessmenEntity, ChessmenPorts chessmenPorts)
        {
            _chessmenEntity = chessmenEntity;
            _chessmenPorts = chessmenPorts;
        }

        public IAsyncSubscriber<ChessmenTransitionVO> transition => _chessmenPorts.chessmenTransitionSubscriber;
        public IAsyncSubscriber<ChessmenMovementVO> movement => _chessmenPorts.chessmenMovementSubscriber;

        public void Init()
        {
            _chessmenEntity.Reset();
        }
        
        public async UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            await _chessmenPorts.chessmenTransitionPublisher
                .PublishAsync(new ChessmenTransitionVO(fade, _chessmenEntity.square), token);
        }
    }
}