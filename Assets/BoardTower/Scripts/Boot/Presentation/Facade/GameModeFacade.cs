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
                Fade.InOut => FadeInOutAsync(gameModeTransition, token),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE)
            });
        }

        public UniTask FadeInAsync(GameModeTransitionVO gameModeTransition, CancellationToken token)
        {
            return _gameModeView.FadeIn(gameModeTransition.transition.duration)
                .ToUniTask(cancellationToken: token);
        }

        public UniTask FadeOutAsync(GameModeTransitionVO gameModeTransition, CancellationToken token)
        {
            return _gameModeView.FadeOut(gameModeTransition.transition.duration)
                .ToUniTask(cancellationToken: token);
        }

        public async UniTask FadeInOutAsync(GameModeTransitionVO gameModeTransition, CancellationToken token)
        {
            await FadeInAsync(gameModeTransition, token);

            await _gameModeView.decision.FirstAsync(cancellationToken: token);

            await FadeOutAsync(gameModeTransition, token);
        }
    }
}