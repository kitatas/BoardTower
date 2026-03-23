using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameSetUpState : BaseGameState
    {
        private readonly BoardUseCase _boardUseCase;
        private readonly ChessmenUseCase _chessmenUseCase;
        private readonly RoundUseCase _roundUseCase;

        public GameSetUpState(BoardUseCase boardUseCase, ChessmenUseCase chessmenUseCase, RoundUseCase roundUseCase)
        {
            _boardUseCase = boardUseCase;
            _chessmenUseCase = chessmenUseCase;
            _roundUseCase = roundUseCase;
        }

        public override GameState state => GameState.SetUp;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await UniTask.Yield(token);
        }

        public override async UniTask EnterAsync(CancellationToken token)
        {
            await (
                _boardUseCase.FadeAsync(Fade.Out, token),
                _chessmenUseCase.FadeAsync(Fade.Out, token)
            );
            _roundUseCase.Increment();

            await _boardUseCase.BuildSquaresAsync(token);
            await _boardUseCase.FadeAsync(Fade.In, token);
            await _chessmenUseCase.FadeAsync(Fade.In, token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            return GameState.Input;
        }
    }
}