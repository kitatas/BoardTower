using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class ScoreRankingFacade
    {
        private readonly ScoreRankingView _scoreRankingView;

        public ScoreRankingFacade(ScoreRankingView scoreRankingView)
        {
            _scoreRankingView = scoreRankingView;
        }

        public UniTask RenderAsync(ScoreRankingVO scoreRanking, CancellationToken token)
        {
            _scoreRankingView.Refresh();
            _scoreRankingView.Render(scoreRanking);
            return UniTask.Yield(token);
        }
    }
}