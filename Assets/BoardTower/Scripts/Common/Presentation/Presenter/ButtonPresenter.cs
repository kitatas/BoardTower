using System;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public sealed class ButtonPresenter : IInitializable, IDisposable
    {
        private readonly SeUseCase _seUseCase;
        private readonly ButtonFacade _buttonFacade;
        private readonly CompositeDisposable _disposable;

        public ButtonPresenter(SeUseCase seUseCase, ButtonFacade buttonFacade)
        {
            _seUseCase = seUseCase;
            _buttonFacade = buttonFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            foreach (var observable in _buttonFacade.OnClickAsObservables())
            {
                observable
                    .Subscribe(x => _seUseCase.Play(x))
                    .AddTo(_disposable);
            }
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}