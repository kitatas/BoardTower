using System;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Boot.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Boot.Presentation.Presenter
{
    public sealed class DisplayNamePresenter : IStartable, IDisposable
    {
        private readonly DisplayNameUseCase _displayNameUseCase;
        private readonly DisplayNameFacade _displayNameFacade;
        private readonly CompositeDisposable _disposable;

        public DisplayNamePresenter(DisplayNameUseCase displayNameUseCase, DisplayNameFacade displayNameFacade)
        {
            _displayNameUseCase = displayNameUseCase;
            _displayNameFacade = displayNameFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _displayNameFacade.OnDecisionDisplayName()
                .Subscribe(_displayNameUseCase.HandleDisplayName)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}