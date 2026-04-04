using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameInitState : BaseGameState
    {
        private readonly ChessmenUseCase _chessmenUseCase;
        private readonly RoundUseCase _roundUseCase;
        private readonly TapScreenUseCase _tapScreenUseCase;

        public GameInitState(ChessmenUseCase chessmenUseCase, RoundUseCase roundUseCase,
            TapScreenUseCase tapScreenUseCase)
        {
            _chessmenUseCase = chessmenUseCase;
            _roundUseCase = roundUseCase;
            _tapScreenUseCase = tapScreenUseCase;
        }

        public override GameState state => GameState.Init;

        public override UniTask EnterAsync(CancellationToken token)
        {
            _chessmenUseCase.Init();
            _roundUseCase.Init();
            return _tapScreenUseCase.FadeAsync(Fade.In, token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _tapScreenUseCase.WaitForTapScreenAsync(token);
            return GameState.SetUp;
        }

        public override UniTask ExitAsync(CancellationToken token)
        {
            return _tapScreenUseCase.FadeAsync(Fade.Out, token);
        }
    }
}