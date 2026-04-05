using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class FinishPresenter : IStartable, IDisposable
    {
        private readonly FinishUseCase _finishUseCase;
        private readonly FinishFacade _finishFacade;
        private readonly CompositeDisposable _disposable;

        public FinishPresenter(FinishUseCase finishUseCase, FinishFacade finishFacade)
        {
            _finishUseCase = finishUseCase;
            _finishFacade = finishFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _finishUseCase.finishTransition
                .Subscribe((f, ct) => _finishFacade.FadeAsync(f, ct))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}