using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Presentation.View;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Presentation.Facade
{
    public sealed class UpdateFacade
    {
        private readonly UpdateView _updateView;

        public UpdateFacade(UpdateView updateView)
        {
            _updateView = updateView;
        }

        public UniTask FadeAsync(UpdateTransitionVO updateTransition, CancellationToken token)
        {
            var tween = updateTransition.transition.fade switch
            {
                Fade.In => _updateView.FadeIn(updateTransition.transition.duration),
                Fade.Out => _updateView.FadeOut(updateTransition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillWithCompleteCallbackAndCancelAwait, token);
        }
    }
}