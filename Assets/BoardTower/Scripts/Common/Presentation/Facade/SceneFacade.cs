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

        public UniTask LoadAsync(LoadVO load, CancellationToken token)
        {
            return load.loadType switch
            {
                LoadType.Direct => LoadSceneAsync(load.sceneName, token),
                LoadType.Fade => FadeLoadAsync(load.sceneName, token),
                _ => throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_LOAD),
            };
        }

        private async UniTask FadeLoadAsync(SceneName sceneName, CancellationToken token)
        {
            await _transitionView.FadeIn(0.1f).ToUniTask(cancellationToken: token);
            await LoadSceneAsync(sceneName, token);
            await _transitionView.FadeOut(0.1f).ToUniTask(cancellationToken: token);
        }

        private static UniTask LoadSceneAsync(SceneName sceneName, CancellationToken token)
        {
            return SceneManager.LoadSceneAsync(sceneName.FastToString())
                .ToUniTask(cancellationToken: token);
        }
    }
}