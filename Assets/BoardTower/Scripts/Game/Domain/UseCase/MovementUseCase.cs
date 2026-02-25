using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;
using BoardTower.Game.Utility;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class MovementUseCase
    {
        private readonly ChessmenEntity _chessmenEntity;
        private readonly ChessmenMovementRepository _chessmenMovementRepository;
        private readonly IAsyncSubscriber<HighlightSquareVO[]> _subscriber;
        private readonly IAsyncPublisher<HighlightSquareVO[]> _publisher;

        public MovementUseCase(ChessmenEntity chessmenEntity, ChessmenMovementRepository chessmenMovementRepository,
            IAsyncSubscriber<HighlightSquareVO[]> subscriber, IAsyncPublisher<HighlightSquareVO[]> publisher)
        {
            _chessmenEntity = chessmenEntity;
            _chessmenMovementRepository = chessmenMovementRepository;
            _subscriber = subscriber;
            _publisher = publisher;
        }

        public IAsyncSubscriber<HighlightSquareVO[]> subscriber => _subscriber;

        public async UniTask PublishMovableSquaresAsync(CancellationToken token)
        {
            var rule = _chessmenMovementRepository.Find(_chessmenEntity.chessmenType);
            var highlights = GetMovableSquares(rule)
                .Select(x => new HighlightSquareVO(x, HighlightSquareType.Movable))
                .ToArray();

            await _publisher.PublishAsync(highlights, token);
        }

        private List<SquareVO> GetMovableSquares(ChessmenMovementRuleVO rule)
        {
            var squares = new List<SquareVO>(16);
            foreach (var offset in rule.offsets)
            {
                for (int step = 1;; step++)
                {
                    var file = _chessmenEntity.file + offset.dx * step;
                    var rank = _chessmenEntity.rank + offset.dy * step;
                    if (BoardHelper.IsOutOfBoard(file, rank)) break;

                    squares.Add(new SquareVO(file, rank));

                    // Slider 以外は1手のみ
                    if (rule.movement is not ChessmenMovementType.Slider) break;
                }
            }

            return squares;
        }
    }
}