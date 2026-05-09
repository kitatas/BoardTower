using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class ScorePresenter : IStartable, IDisposable
    {
        private readonly ScoreUseCase _scoreUseCase;
        private readonly ScoreFacade _scoreFacade;
        private readonly CompositeDisposable _disposable;

        public ScorePresenter(ScoreUseCase scoreUseCase, ScoreFacade scoreFacade)
        {
            _scoreUseCase = scoreUseCase;
            _scoreFacade = scoreFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _scoreUseCase.score
                .DistinctUntilChanged()
                .Pairwise()
                .Subscribe(_scoreFacade.Render)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}