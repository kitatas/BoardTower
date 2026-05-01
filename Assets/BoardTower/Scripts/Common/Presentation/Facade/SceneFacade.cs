using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View;
using BoardTower.Common.Utility;
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

        public async UniTask LoadSceneAsync(SceneName sceneName, CancellationToken token)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName.FastToString());
            if (asyncOperation == null) throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_LOAD_SCENE);

            asyncOperation.allowSceneActivation = false;
            await UniTask.WaitUntil(() => asyncOperation.progress >= SceneConfig.LOAD_PROGRESS_THRESHOLD,
                PlayerLoopTiming.Update, token);

            GC.Collect();

            asyncOperation.allowSceneActivation = true;
            await asyncOperation
                .ToUniTask(cancellationToken: token);

            for (int i = 0; i < SceneConfig.STABILITY_FRAME; i++)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            await UniTaskHelper.DelayAsync(SceneConfig.STABILITY_TIME, token);
        }
    }
}