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

        public async UniTask<bool> ApplyEventAsync(CancellationToken token)
        {
            var squareEvent = _boardEntity.FindEvent(_chessmenEntity.square);
            if (squareEvent.type.IsBeltEvent())
            {
                await BeltAsync(squareEvent.type, token);
                return true; // 移動後の SquareEvent 実行
            }

            return false;
        }

        private UniTask BeltAsync(SquareEventType type, CancellationToken token)
        {
            _chessmenEntity.MoveBy(type.ToBeltOffset());
            return _eventPorts.movementPublisher.PublishAsync(_chessmenEntity.movement, token);
        }
    }
}