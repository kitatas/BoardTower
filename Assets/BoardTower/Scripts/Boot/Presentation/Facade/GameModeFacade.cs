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
            await (gameModeTransition.transition.fade switch
            {
                Fade.In => FadeInAsync(gameModeTransition, token),
                Fade.Out => FadeOutAsync(gameModeTransition, token),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE)
            });
        }

        public async UniTask FadeInAsync(GameModeTransitionVO gameModeTransition, CancellationToken token)
        {
            await _gameModeView.FadeIn(gameModeTransition.transition.duration)
                .ToUniTask(cancellationToken: token);

            await _gameModeView.decision.FirstAsync(cancellationToken: token);

            await FadeOutAsync(gameModeTransition, token);
        }

        public UniTask FadeOutAsync(GameModeTransitionVO gameModeTransition, CancellationToken token)
        {
            return _gameModeView.FadeOut(gameModeTransition.transition.duration)
                .ToUniTask(cancellationToken: token);
        }
    }
}