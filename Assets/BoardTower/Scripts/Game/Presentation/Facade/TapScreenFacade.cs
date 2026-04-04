using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class TapScreenFacade
    {
        private readonly TapScreenView _tapScreenView;

        public TapScreenFacade(TapScreenView tapScreenView)
        {
            _tapScreenView = tapScreenView;
        }

        public Observable<Unit> OnTapAsObservable() => _tapScreenView.tap;

        public UniTask FadeAsync(TapScreenVO tapScreen, CancellationToken token)
        {
            var tween = tapScreen.transition.fade switch
            {
                Fade.In => _tapScreenView.FadeIn(tapScreen.transition.duration),
                Fade.Out => _tapScreenView.FadeOut(tapScreen.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}