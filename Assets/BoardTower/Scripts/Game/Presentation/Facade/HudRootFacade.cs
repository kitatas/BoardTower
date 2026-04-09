using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class HudRootFacade
    {
        private readonly HudRootView _hudRootView;

        public HudRootFacade(HudRootView hudRootView)
        {
            _hudRootView = hudRootView;
        }

        public UniTask FadeAsync(HudRootTransitionVO hudRootTransition, CancellationToken token)
        {
            var tween = hudRootTransition.transition.fade switch
            {
                Fade.In => _hudRootView.FadeIn(hudRootTransition.transition.duration),
                Fade.Out => _hudRootView.FadeOut(hudRootTransition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}