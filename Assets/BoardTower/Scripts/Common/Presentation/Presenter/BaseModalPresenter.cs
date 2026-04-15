using System;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public abstract class BaseModalPresenter<T> : IInitializable, IDisposable where T : Enum
    {
        private readonly BaseModalUseCase<T> _modalUseCase;
        private readonly BaseModalFacade<T> _modalFacade;
        private readonly CompositeDisposable _disposable;

        public BaseModalPresenter(BaseModalUseCase<T> modalUseCase, BaseModalFacade<T> modalFacade)
        {
            _modalUseCase = modalUseCase;
            _modalFacade = modalFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _modalUseCase.subscriber
                .Subscribe((t, ct) => _modalFacade.FadeAsync(t, ct))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}