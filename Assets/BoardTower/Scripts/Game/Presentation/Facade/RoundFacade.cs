using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class RoundFacade
    {
        private readonly RoundView _roundView;

        public RoundFacade(RoundView roundView)
        {
            _roundView = roundView;
        }

        public void Render((int prev, int current) pair)
        {
            _roundView.Render(pair.prev, pair.current, UiConfig.DURATION);
        }
    }
}