using System.Threading;
using BoardTower.Common.Application;
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

        public async UniTask<EventResultVO> ApplyEventAsync(CancellationToken token)
        {
            var (squareEvent, index) = _boardEntity.FindEvent(_chessmenEntity.square);
            await (squareEvent.type switch
            {
                SquareEventType.Empty => UniTask.Yield(token),
                SquareEventType.Gem => OverrideSquareEventAsync(index, SquareEventType.Empty, token),
                SquareEventType.Ply => OverrideSquareEventAsync(index, SquareEventType.Empty, token),
                var type when type.IsBeltEvent() => BeltAsync(squareEvent.type, token),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT),
            });

            return new EventResultVO(
                squareEvent.type.IsBeltEvent(),
                squareEvent.type == SquareEventType.Gem ? 1 : 0,
                squareEvent.type == SquareEventType.Ply ? 1 : 0
            );
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