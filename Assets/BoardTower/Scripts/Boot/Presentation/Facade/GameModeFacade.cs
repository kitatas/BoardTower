using System.Threading;
using BoardTower.Boot.Presentation.View;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Boot.Presentation.Facade
{
    public sealed class GameModeFacade
    {
        private readonly GameModeView _gameModeView;

        public GameModeFacade(GameModeView gameModeView)
        {
            _gameModeView = gameModeView;
        }

        public async UniTask FadeAsync(GameModeTransitionVO gameModeTransition, CancellationToken token)
        {
            await _gameModeView.FadeIn(gameModeTransition.transition.duration)
                .ToUniTask(cancellationToken: token);

            await _gameModeView.decision.FirstAsync(cancellationToken: token);

            await _gameModeView.FadeOut(gameModeTransition.transition.duration)
                .ToUniTask(cancellationToken: token);
        }
    }
}