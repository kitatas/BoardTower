using System;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public sealed class ScenePresenter : IInitializable, IDisposable
    {
        private readonly SceneUseCase _sceneUseCase;
        private readonly SceneFacade _sceneFacade;
        private readonly CompositeDisposable _disposable;

        public ScenePresenter(SceneUseCase sceneUseCase, SceneFacade sceneFacade)
        {
            _sceneUseCase = sceneUseCase;
            _sceneFacade = sceneFacade;
            _disposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _sceneUseCase.load
                .SubscribeAwait(async (x, ct) => await _sceneFacade.LoadAsync(x, ct))
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}