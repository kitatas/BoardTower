using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Common.Presentation.Facade
{
    public sealed class ExceptionFacade
    {
        private readonly ExceptionView _exceptionView;

        public ExceptionFacade(ExceptionView exceptionView)
        {
            _exceptionView = exceptionView;
        }

        public Observable<Unit> OnDecision() => _exceptionView.decision;

        public UniTask FadeAsync(ExceptionNotifyVO notify, CancellationToken token)
        {
            var tween = notify.transition.fade switch
            {
                Fade.In => _exceptionView.FadeIn(notify.exception?.message ?? ExceptionConfig.UNKNOWN_ERROR, notify.transition.duration),
                Fade.Out => _exceptionView.FadeOut(notify.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}