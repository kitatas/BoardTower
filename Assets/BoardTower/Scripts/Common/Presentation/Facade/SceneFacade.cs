using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View;
using Cysharp.Threading.Tasks;
using FastEnumUtility;
using UnityEngine.SceneManagement;

namespace BoardTower.Common.Presentation.Facade
{
    public sealed class SceneFacade
    {
        private readonly TransitionView _transitionView;

        public SceneFacade(TransitionView transitionView)
        {
            _transitionView = transitionView;
        }

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var tween = fade switch
            {
                Fade.In => _transitionView.FadeIn(SceneConfig.FADE_DURATION),
                Fade.Out => _transitionView.FadeOut(SceneConfig.FADE_DURATION),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        public UniTask LoadSceneAsync(SceneName sceneName, CancellationToken token)
        {
            return SceneManager.LoadSceneAsync(sceneName.FastToString())
                .ToUniTask(cancellationToken: token);
        }
    }
}