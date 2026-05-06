using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class LotRelicFacade
    {
        private readonly LotRelicView _lotRelicView;

        public LotRelicFacade(LotRelicView lotRelicView)
        {
            _lotRelicView = lotRelicView;
        }

        public void Render(LotRelicVO lotRelic)
        {
            _lotRelicView.Render(lotRelic);
        }

        public UniTask FadeAsync(LotRelicTransitionVO lotRelicTransition, CancellationToken token)
        {
            var tween = lotRelicTransition.transition.fade switch
            {
                Fade.In => _lotRelicView.FadeIn(lotRelicTransition.transition.duration),
                Fade.Out => _lotRelicView.FadeOut(lotRelicTransition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        public Observable<SelectRelicVO> OnClickAnyAsObservable()
        {
            return _lotRelicView.OnClickAnyAsObservable();
        }
    }
}