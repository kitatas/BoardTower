using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class GemFacade
    {
        private readonly GemView _gemView;

        public GemFacade(GemView gemView)
        {
            _gemView = gemView;
        }

        public void Render((int prev, int current) pair)
        {
            _gemView.Render(pair.prev, pair.current, UiConfig.DURATION);
        }
    }
}