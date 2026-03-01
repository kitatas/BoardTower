using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class EventUseCase
    {
        private readonly BoardEntity _boardEntity;
        private readonly ChessmenEntity _chessmenEntity;
        private readonly EventPorts _eventPorts;

        public EventUseCase(BoardEntity boardEntity, ChessmenEntity chessmenEntity, EventPorts eventPorts)
        {
            _boardEntity = boardEntity;
            _chessmenEntity = chessmenEntity;
            _eventPorts = eventPorts;
        }

        public async UniTask ApplyEventAsync(CancellationToken token)
        {
            var squareEvent = _boardEntity.FindEvent(_chessmenEntity.square);
            if (squareEvent.type.IsBeltEvent())
            {
                await BeltAsync(squareEvent.type, token);
            }
        }

        private UniTask BeltAsync(SquareEventType type, CancellationToken token)
        {
            var (dFile, dRank) = type.ToBeltFileRank();
            var square = new SquareVO(_chessmenEntity.square.file + dFile, _chessmenEntity.square.rank + dRank);
            _chessmenEntity.Set(square);

            var movementVo = new ChessmenMovementVO(_chessmenEntity.square);
            return _eventPorts.movementPublisher.PublishAsync(movementVo, token);
        }
    }
}