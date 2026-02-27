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
        private readonly GameStateEntity _gameStateEntity;
        private readonly MovementPorts _movementPorts;
        private readonly ChessmenMovementRepository _chessmenMovementRepository;

        public MovementUseCase(ChessmenEntity chessmenEntity, GameStateEntity gameStateEntity,
            MovementPorts movementPorts, ChessmenMovementRepository chessmenMovementRepository)
        {
            _chessmenEntity = chessmenEntity;
            _gameStateEntity = gameStateEntity;
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

        public async UniTask HandleClickAsync(ClickSquareVO clickSquare, CancellationToken token)
        {
            // GameState.Input 以外は処理させない
            if (!_gameStateEntity.IsEqual(GameState.Input)) return;

            // 移動可能範囲外であれば処理させない
            var rule = _chessmenMovementRepository.Find(_chessmenEntity.chessmenType);
            var squares = BoardHelper.GetMovableSquares(_chessmenEntity.square, rule);
            if (!squares.Any(x => x.IsEqual(clickSquare.square))) return;

            var highlightVos = squares
                .Select(x => new HighlightSquareVO(x, HighlightSquareType.Default))
                .ToArray();

            await _movementPorts.highlightsPublisher.PublishAsync(highlightVos, token);

            _chessmenEntity.Set(clickSquare.square);
            // TODO: 更新したマス目への移動通知
        }
    }
}