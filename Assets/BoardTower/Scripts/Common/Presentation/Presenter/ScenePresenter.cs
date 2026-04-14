using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using Cysharp.Threading.Tasks;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public sealed class ScenePresenter : IInitializable, IDisposable
    {
        private readonly LoadingUseCase _loadingUseCase;
        private readonly SceneUseCase _sceneUseCase;
        private readonly SceneFacade _sceneFacade;
        private readonly CompositeDisposable _disposable;

        public ScenePresenter(LoadingUseCase loadingUseCase, SceneUseCase sceneUseCase, SceneFacade sceneFacade)
        {
            _loadingUseCase = loadingUseCase;
            _sceneUseCase = sceneUseCase;
            _sceneFacade = sceneFacade;
            _disposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _sceneUseCase.load
                .SubscribeAwait(async (x, ct) =>
                {
                    if (x.loadType is LoadType.Fade) await FadeAsync(Fade.In, ct);
                    await _sceneFacade.LoadSceneAsync(x.sceneName, ct);
                    if (x.loadType is LoadType.Fade) await FadeAsync(Fade.Out, ct);
                })
                .AddTo(_disposable);
        }

        private async UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            await (
                _loadingUseCase.FadeAsync(fade, token),
                _sceneFacade.FadeAsync(fade, token)
            );
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}