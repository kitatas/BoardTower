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
        private readonly RoundUseCase _roundUseCase;
        private readonly TapScreenUseCase _tapScreenUseCase;

        public GameInitState(BgmUseCase bgmUseCase, ChessmenUseCase chessmenUseCase, HudRootUseCase hudRootUseCase,
            RoundUseCase roundUseCase, TapScreenUseCase tapScreenUseCase)
        {
            _bgmUseCase = bgmUseCase;
            _chessmenUseCase = chessmenUseCase;
            _hudRootUseCase = hudRootUseCase;
            _roundUseCase = roundUseCase;
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
            _roundUseCase.Init();
            await (
                _hudRootUseCase.FadeAsync(Fade.Out, token),
                _tapScreenUseCase.FadeAsync(Fade.In, token)
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