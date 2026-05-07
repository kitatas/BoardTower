using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class SelectRelicFacade
    {
        private readonly SelectRelicView _selectRelicView;

        public SelectRelicFacade(SelectRelicView selectRelicView)
        {
            _selectRelicView = selectRelicView;
        }

        public void Render(SelectRelicVO selectRelic)
        {
            _selectRelicView.Render(selectRelic.position);
        }

        public void Hide()
        {
            _selectRelicView.Hide();
        }
    }
}