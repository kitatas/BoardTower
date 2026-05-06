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

        public bool IsEqualPosition(SelectRelicVO selectRelic)
        {
            return _selectRelicView.IsEqualPosition(selectRelic.position);
        }

        public void SetPosition(SelectRelicVO selectRelic)
        {
            _selectRelicView.SetPosition(selectRelic.position);
        }
    }
}