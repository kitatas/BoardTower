using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class ScoreFacade
    {
        private readonly ScoreView _scoreView;

        public ScoreFacade(ScoreView scoreView)
        {
            _scoreView = scoreView;
        }

        public void Render((int prev, int current) pair)
        {
            _scoreView.Render(pair.prev, pair.current, UiConfig.DURATION);
        }
    }
}