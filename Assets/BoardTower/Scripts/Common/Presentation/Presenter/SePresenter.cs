using System;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public sealed class SePresenter : IInitializable, IDisposable
    {
        private readonly SeUseCase _seUseCase;
        private readonly SeFacade _seFacade;
        private readonly CompositeDisposable _disposable;

        public SePresenter(SeUseCase seUseCase, SeFacade seFacade)
        {
            _seUseCase = seUseCase;
            _seFacade = seFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _seUseCase.play
                .Subscribe(_seFacade.Play)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}