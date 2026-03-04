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
        private readonly SquareEventRepository _squareEventRepository;

        public BoardUseCase(BoardEntity boardEntity, BoardPorts boardPorts, SquareEventRepository squareEventRepository)
        {
            _boardEntity = boardEntity;
            _boardPorts = boardPorts;
            _squareEventRepository = squareEventRepository;
        }

        public IAsyncSubscriber<BoardTransitionVO> transition => _boardPorts.boardTransitionSubscriber;
        public IAsyncSubscriber<EventSquareVO[]> eventSquares => _boardPorts.eventSquaresSubscriber;

        public async UniTask InitAsync(CancellationToken token)
        {
            await _boardPorts.boardTransitionPublisher
                .PublishAsync(new BoardTransitionVO(Fade.Out), token);
        }

        public async UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            await _boardPorts.boardTransitionPublisher
                .PublishAsync(new BoardTransitionVO(fade, BoardConfig.FADE_DURATION), token);

            // TODO: 仮
            var types = fade switch
            {
                Fade.In => new[]
                {
                    SquareEventType.Empty, SquareEventType.Empty, SquareEventType.Empty, SquareEventType.Empty,
                    SquareEventType.BeltUp, SquareEventType.BeltDown,
                    SquareEventType.BeltLeft, SquareEventType.BeltRight
                },
                Fade.Out => new[] { SquareEventType.Empty },
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };
            for (int i = BoardConfig.MIN_FILE; i <= BoardConfig.MAX_FILE; i++)
            {
                for (int j = BoardConfig.MIN_RANK; j <= BoardConfig.MAX_RANK; j++)
                {
                    var type = types[UnityEngine.Random.Range(0, types.Length)];
                    var squareEvent = _squareEventRepository.Find(type);
                    _boardEntity.Add(new EventSquareVO(new SquareVO(i, j), squareEvent));
                }
            }

            await _boardPorts.eventSquaresPublisher
                .PublishAsync(_boardEntity.events, token);
        }
    }
}