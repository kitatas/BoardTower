using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class LotRelicFacade
    {
        private readonly LotRelicView _lotRelicView;

        public LotRelicFacade(LotRelicView lotRelicView)
        {
            _lotRelicView = lotRelicView;
        }

        public void Render(LotRelicVO lotRelic)
        {
            _lotRelicView.Render(lotRelic);
        }
    }
}