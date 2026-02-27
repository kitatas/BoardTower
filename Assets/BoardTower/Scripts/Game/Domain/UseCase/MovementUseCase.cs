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
            var highlights = BoardHelper.GetMovableSquares(_chessmenEntity.square, rule)
                .Select(x => new HighlightSquareVO(x, HighlightSquareType.Movable))
                .ToArray();

            await _publisher.PublishAsync(highlights, token);
        }
    }
}