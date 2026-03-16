using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class RoundFacade
    {
        private readonly RoundView _roundView;
        private readonly RoundMaxNumView _roundMaxNumView;

        public RoundFacade(RoundView roundView, RoundMaxNumView roundMaxNumView)
        {
            _roundView = roundView;
            _roundMaxNumView = roundMaxNumView;
        }

        public void Render((int prev, int current) pair)
        {
            _roundView.Render(pair.prev, pair.current, UiConfig.DURATION);
        }

        public void RenderMax((int prev, int current) pair)
        {
            _roundMaxNumView.Render(pair.prev, pair.current, UiConfig.DURATION);
        }
    }
}