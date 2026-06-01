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
        private readonly AccountUseCase _accountUseCase;
        private readonly SceneUseCase _sceneUseCase;
        private readonly DeleteFacade _deleteFacade;
        private readonly CompositeDisposable _disposable;

        public DeletePresenter(AccountUseCase accountUseCase, SceneUseCase sceneUseCase, DeleteFacade deleteFacade)
        {
            _accountUseCase = accountUseCase;
            _sceneUseCase = sceneUseCase;
            _deleteFacade = deleteFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _deleteFacade.clickDecision
                .Subscribe(_ => _accountUseCase.Delete())
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