using System.Linq;
using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.Repository;
using BoardTower.Game.Utility;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class MovementUseCase
    {
        private readonly ChessmenEntity _chessmenEntity;
        private readonly MovementPorts _movementPorts;
        private readonly ChessmenMovementRepository _chessmenMovementRepository;

        public MovementUseCase(ChessmenEntity chessmenEntity, MovementPorts movementPorts,
            ChessmenMovementRepository chessmenMovementRepository)
        {
            _chessmenEntity = chessmenEntity;
            _movementPorts = movementPorts;
            _chessmenMovementRepository = chessmenMovementRepository;
        }

        public IAsyncSubscriber<HighlightSquareVO[]> highlights => _movementPorts.highlightsSubscriber;

        public async UniTask PublishMovableSquaresAsync(CancellationToken token)
        {
            var rule = _chessmenMovementRepository.Find(_chessmenEntity.chessmenType);
            var highlightVos = BoardHelper.GetMovableSquares(_chessmenEntity.square, rule)
                .Select(x => new HighlightSquareVO(x, HighlightSquareType.Movable))
                .ToArray();

            await _movementPorts.highlightsPublisher.PublishAsync(highlightVos, token);
        }
    }
}