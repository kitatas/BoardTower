using System;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class DeletePresenter : IStartable, IDisposable
    {
        private readonly SceneUseCase _sceneUseCase;
        private readonly UserDataUseCase _userDataUseCase;
        private readonly DeleteFacade _deleteFacade;
        private readonly CompositeDisposable _disposable;

        public DeletePresenter(SceneUseCase sceneUseCase, UserDataUseCase userDataUseCase, DeleteFacade deleteFacade)
        {
            _sceneUseCase = sceneUseCase;
            _userDataUseCase = userDataUseCase;
            _deleteFacade = deleteFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _deleteFacade.clickDecision
                .Subscribe(_ => _userDataUseCase.Delete())
                .AddTo(_disposable);

            _deleteFacade.clickBackTitle
                .Subscribe(_ => _sceneUseCase.Load(SceneName.Boot, LoadType.Fade))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}