using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Presentation.View;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Presentation.Facade
{
    public sealed class DisplayNameFacade
    {
        private readonly DisplayNameView _displayNameView;

        public DisplayNameFacade(DisplayNameView displayNameView)
        {
            _displayNameView = displayNameView;
        }

        public UniTask FadeAsync(DisplayNameTransitionVO displayNameTransition, CancellationToken token)
        {
            var tween = displayNameTransition.transition.fade switch
            {
                Fade.In => _displayNameView.FadeIn(displayNameTransition.transition.duration),
                Fade.Out => _displayNameView.FadeOut(displayNameTransition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(cancellationToken: token);
        }
    }
}