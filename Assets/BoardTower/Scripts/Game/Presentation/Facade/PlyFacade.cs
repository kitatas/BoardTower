using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class PlyFacade
    {
        private readonly PlyView _plyView;

        public PlyFacade(PlyView plyView)
        {
            _plyView = plyView;
        }

        public void Render((int prev, int current) pair)
        {
            _plyView.Render(pair.prev, pair.current, UiConfig.DURATION);
        }
    }
}