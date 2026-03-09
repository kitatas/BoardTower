using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class GemPresenter : IStartable, IDisposable
    {
        private readonly GemUseCase _gemUseCase;
        private readonly GemFacade _gemFacade;
        private readonly CompositeDisposable _disposable;

        public GemPresenter(GemUseCase gemUseCase, GemFacade gemFacade)
        {
            _gemUseCase = gemUseCase;
            _gemFacade = gemFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _gemUseCase.gem
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_gemFacade.Render)
                .AddTo(_disposable);

            _gemUseCase.Init();
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}