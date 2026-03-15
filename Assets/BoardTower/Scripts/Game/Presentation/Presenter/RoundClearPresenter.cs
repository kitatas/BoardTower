using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class RoundClearPresenter : IStartable, IDisposable
    {
        private readonly RoundClearUseCase _roundClearUseCase;
        private readonly RoundClearFacade _roundClearFacade;
        private readonly CompositeDisposable _disposable;

        public RoundClearPresenter(RoundClearUseCase roundClearUseCase, RoundClearFacade roundClearFacade)
        {
            _roundClearUseCase = roundClearUseCase;
            _roundClearFacade = roundClearFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _roundClearUseCase.roundClear
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_roundClearFacade.Render)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}