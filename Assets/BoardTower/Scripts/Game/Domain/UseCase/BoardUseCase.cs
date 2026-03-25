using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.Repository;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class BoardUseCase
    {
        private readonly BoardEntity _boardEntity;
        private readonly BoardPorts _boardPorts;
        private readonly BoardPatternRepository _boardPatternRepository;
        private readonly SquareEventRepository _squareEventRepository;

        public BoardUseCase(BoardEntity boardEntity, BoardPorts boardPorts,
            BoardPatternRepository boardPatternRepository, SquareEventRepository squareEventRepository)
        {
            _boardEntity = boardEntity;
            _boardPorts = boardPorts;
            _boardPatternRepository = boardPatternRepository;
            _squareEventRepository = squareEventRepository;
        }

        public IAsyncSubscriber<BoardTransitionVO> transition => _boardPorts.boardTransitionSubscriber;
        public IAsyncSubscriber<EventSquareVO[]> eventSquares => _boardPorts.eventSquaresSubscriber;

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            return _boardPorts.PublishBoardTransitionAsync(new BoardTransitionVO(new TransitionVO(fade, BoardConfig.FADE_DURATION)), token);
        }

        public UniTask BuildSquaresAsync(CancellationToken token)
        {
            _boardEntity.Clear();

            // 4x4 pattern から board 生成
            var patterns = _boardPatternRepository.GetRandomPatterns();
            for (int file = 0; file < BoardConfig.MAX_FILE; file++)
            {
                var patternOffset = (file / BoardConfig.HALF_FILE) * (BoardConfig.MAX_FILE / BoardConfig.HALF_FILE);
                var typeOffset = (file % BoardConfig.HALF_FILE) * BoardConfig.HALF_FILE;

                for (int rank = 0; rank < BoardConfig.MAX_RANK; rank++)
                {
                    var patternIndex = (rank / BoardConfig.HALF_RANK) + patternOffset;
                    var pattern = patterns[patternIndex];

                    var typeIndex = (rank % BoardConfig.HALF_RANK) + typeOffset;
                    var type = pattern.types[typeIndex];

                    var squareEvent = _squareEventRepository.Find(type);
                    _boardEntity.Add(new EventSquareVO(new SquareVO(file + 1, rank + 1), squareEvent));
                }
            }

            return _boardPorts.PublishEventSquaresAsync(_boardEntity.events, token);
        }
    }
}