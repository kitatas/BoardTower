using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Utility;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameFailState : BaseGameState
    {
        private readonly FinishUseCase _finishUseCase;

        public GameFailState(FinishUseCase finishUseCase)
        {
            _finishUseCase = finishUseCase;
        }

        public override GameState state => GameState.Fail;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await (
                _finishUseCase.InitAsync(FinishType.Fail, token)
            );
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _finishUseCase.FadeAsync(FinishType.Fail, Fade.In, token);
            await UniTaskHelper.DelayAsync(1.0f, token);
            await _finishUseCase.FadeAsync(FinishType.Fail, Fade.Out, token);

            return GameState.Finish;
        }

        public override async UniTask ForceExitAsync(CancellationToken token)
        {
            await (
                _finishUseCase.FadeAsync(FinishType.Fail, Fade.Out, token)
            );
        }
    }
}