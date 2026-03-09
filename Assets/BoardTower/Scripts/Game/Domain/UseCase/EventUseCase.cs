using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.Repository;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class EventUseCase
    {
        private readonly BoardEntity _boardEntity;
        private readonly ChessmenEntity _chessmenEntity;
        private readonly EventPorts _eventPorts;
        private readonly SquareEventRepository _squareEventRepository;

        public EventUseCase(BoardEntity boardEntity, ChessmenEntity chessmenEntity, EventPorts eventPorts,
            SquareEventRepository squareEventRepository)
        {
            _boardEntity = boardEntity;
            _chessmenEntity = chessmenEntity;
            _eventPorts = eventPorts;
            _squareEventRepository = squareEventRepository;
        }

        public async UniTask<bool> ApplyEventAsync(CancellationToken token)
        {
            var (squareEvent, index) = _boardEntity.FindEvent(_chessmenEntity.square);
            if (squareEvent.type.IsBeltEvent())
            {
                await BeltAsync(squareEvent.type, token);
                return true; // 移動後の SquareEvent 実行
            }

            if (squareEvent.type.IsOverrideEmptyEvent())
            {
                await OverrideSquareEventAsync(index, SquareEventType.Empty, token);
                return false;
            }

            return false;
        }

        private UniTask BeltAsync(SquareEventType type, CancellationToken token)
        {
            _chessmenEntity.MoveBy(type.ToBeltOffset());
            return _eventPorts.movementPublisher.PublishAsync(_chessmenEntity.movement, token);
        }

        private UniTask OverrideSquareEventAsync(int index, SquareEventType type, CancellationToken token)
        {
            _boardEntity.Set(index, _squareEventRepository.Find(type));
            return _eventPorts.eventSquaresPublisher.PublishAsync(_boardEntity.events, token);
        }
    }
}