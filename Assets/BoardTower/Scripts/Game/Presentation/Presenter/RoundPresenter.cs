using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class RoundPresenter : IStartable, IDisposable
    {
        private readonly RoundUseCase _roundUseCase;
        private readonly RoundFacade _roundFacade;
        private readonly CompositeDisposable _disposable;

        public RoundPresenter(RoundUseCase roundUseCase, RoundFacade roundFacade)
        {
            _roundUseCase = roundUseCase;
            _roundFacade = roundFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _roundUseCase.round
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_roundFacade.Render)
                .AddTo(_disposable);

            _roundUseCase.Init();
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}