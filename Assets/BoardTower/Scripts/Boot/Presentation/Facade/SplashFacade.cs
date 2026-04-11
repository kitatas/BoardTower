using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Presentation.View;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Boot.Presentation.Facade
{
    public sealed class SplashFacade
    {
        private readonly SplashView _splashView;

        public SplashFacade(SplashView splashView)
        {
            _splashView = splashView;
        }

        public Observable<Unit> OnTapAsObservable() => _splashView.tap;

        public UniTask FadeAsync(SplashTransitionVO splashTransition, CancellationToken token)
        {
            var tween = splashTransition.transition.fade switch
            {
                Fade.In => _splashView.FadeIn(splashTransition.splash.sprite, splashTransition.transition.duration),
                Fade.Out => _splashView.FadeOut(splashTransition.transition.duration),
                Fade.InOut => _splashView.FadeInOut(splashTransition.splash.sprite, splashTransition.transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }
    }
}