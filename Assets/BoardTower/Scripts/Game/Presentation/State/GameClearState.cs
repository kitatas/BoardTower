using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Utility;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameClearState : BaseGameState
    {
        private readonly FinishUseCase _finishUseCase;

        public GameClearState(FinishUseCase finishUseCase)
        {
            _finishUseCase = finishUseCase;
        }

        public override GameState state => GameState.Clear;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await (
                _finishUseCase.InitAsync(FinishType.Clear, token)
            );
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _finishUseCase.FadeAsync(FinishType.Clear, Fade.In, token);
            await UniTaskHelper.DelayAsync(1.0f, token);
            await _finishUseCase.FadeAsync(FinishType.Clear, Fade.Out, token);

            return GameState.Finish;
        }
    }
}