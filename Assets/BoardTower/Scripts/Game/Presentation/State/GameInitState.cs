using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameInitState : BaseGameState
    {
        private readonly BgmUseCase _bgmUseCase;
        private readonly ChessmenUseCase _chessmenUseCase;
        private readonly HudRootUseCase _hudRootUseCase;
        private readonly PickRelicUseCase _pickRelicUseCase;
        private readonly RoundUseCase _roundUseCase;
        private readonly ScoreUseCase _scoreUseCase;
        private readonly TapScreenUseCase _tapScreenUseCase;

        public GameInitState(BgmUseCase bgmUseCase, ChessmenUseCase chessmenUseCase, HudRootUseCase hudRootUseCase,
            PickRelicUseCase pickRelicUseCase, RoundUseCase roundUseCase, ScoreUseCase scoreUseCase,
            TapScreenUseCase tapScreenUseCase)
        {
            _bgmUseCase = bgmUseCase;
            _chessmenUseCase = chessmenUseCase;
            _hudRootUseCase = hudRootUseCase;
            _pickRelicUseCase = pickRelicUseCase;
            _roundUseCase = roundUseCase;
            _scoreUseCase = scoreUseCase;
            _tapScreenUseCase = tapScreenUseCase;
        }

        public override GameState state => GameState.Init;

        public override UniTask InitAsync(CancellationToken token)
        {
            return _hudRootUseCase.InitAsync(token);
        }

        public override async UniTask EnterAsync(CancellationToken token)
        {
            _bgmUseCase.Play(BgmType.Top);
            _chessmenUseCase.Init();
            await (
                _hudRootUseCase.FadeAsync(Fade.Out, token),
                _tapScreenUseCase.FadeAsync(Fade.In, token)
            );

            _roundUseCase.Init();
            _scoreUseCase.Init();
            await (
                _pickRelicUseCase.InitAsync(token)
            );
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _tapScreenUseCase.WaitForTapScreenAsync(token);
            return GameState.SetUp;
        }

        public override async UniTask ExitAsync(CancellationToken token)
        {
            await (
                _hudRootUseCase.FadeAsync(Fade.In, token),
                _tapScreenUseCase.FadeAsync(Fade.Out, token)
            );

            _bgmUseCase.Play(BgmType.Game);
        }
    }
}