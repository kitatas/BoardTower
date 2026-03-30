using System;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public sealed class ExceptionPresenter : IInitializable, IDisposable
    {
        private readonly ExceptionUseCase _exceptionUseCase;
        private readonly ExceptionFacade _exceptionFacade;
        private readonly CompositeDisposable _disposable;

        public ExceptionPresenter(ExceptionUseCase exceptionUseCase, ExceptionFacade exceptionFacade)
        {
            _exceptionUseCase = exceptionUseCase;
            _exceptionFacade = exceptionFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _exceptionUseCase.exception
                .Subscribe((e, ct) => _exceptionFacade.FadeAsync(e, ct))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}