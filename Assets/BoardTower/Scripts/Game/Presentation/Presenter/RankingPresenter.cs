using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class RankingPresenter : IStartable, IDisposable
    {
        private readonly RankingUseCase _rankingUseCase;
        private readonly ScoreRankingFacade _scoreRankingFacade;
        private readonly CompositeDisposable _disposable;

        public RankingPresenter(RankingUseCase rankingUseCase, ScoreRankingFacade scoreRankingFacade)
        {
            _rankingUseCase = rankingUseCase;
            _scoreRankingFacade = scoreRankingFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _rankingUseCase.scoreRanking
                .Subscribe(_scoreRankingFacade.RenderAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}