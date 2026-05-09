using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class GemComboFacade
    {
        private readonly GemComboView _gemComboView;

        public GemComboFacade(GemComboView gemComboView)
        {
            _gemComboView = gemComboView;
        }

        public void Render((int prev, int current) pair)
        {
            if (pair.current > 0)
            {
                _gemComboView.Render(pair.current);
            }
            else
            {
                _gemComboView.RenderReset();
            }
        }
    }
}