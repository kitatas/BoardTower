using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class RoundClearFacade
    {
        private readonly RoundClearGemCountView _roundClearGemCountView;

        public RoundClearFacade(RoundClearGemCountView roundClearGemCountView)
        {
            _roundClearGemCountView = roundClearGemCountView;
        }

        public void Render((int prev, int current) pair)
        {
            _roundClearGemCountView.Render(pair.prev, pair.current, UiConfig.DURATION);
        }
    }
}