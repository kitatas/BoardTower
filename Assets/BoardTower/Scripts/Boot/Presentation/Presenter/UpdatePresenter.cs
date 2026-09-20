using System;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Boot.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Boot.Presentation.Presenter
{
    public sealed class UpdatePresenter : IStartable, IDisposable
    {
        private readonly UpdateUseCase _updateUseCase;
        private readonly UpdateFacade _updateFacade;
        private readonly CompositeDisposable _disposable;

        public UpdatePresenter(UpdateUseCase updateUseCase, UpdateFacade updateFacade)
        {
            _updateUseCase = updateUseCase;
            _updateFacade = updateFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _updateUseCase.transition
                .Subscribe(_updateFacade.FadeAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}