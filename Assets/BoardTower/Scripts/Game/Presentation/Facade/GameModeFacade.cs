using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class GameModeFacade
    {
        private readonly GameModeView _gameModeView;

        public GameModeFacade(GameModeView gameModeView)
        {
            _gameModeView = gameModeView;
        }

        public UniTask FadeAsync(GameModeTransitionVO gameModeTransition, CancellationToken token)
        {
            var tween = gameModeTransition.transition.fade switch
            {
                Fade.In => _gameModeView.FadeIn(gameModeTransition.gameMode, gameModeTransition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}