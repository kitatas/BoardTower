using BoardTower.Game.Presentation.View;
using R3;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class GiveUpFacade
    {
        private readonly GiveUpView _giveUpView;

        public GiveUpFacade(GiveUpView giveUpView)
        {
            _giveUpView = giveUpView;
        }

        public Observable<Unit> clickDecision => _giveUpView.clickDecision;
        public Observable<Unit> clickBackTop => _giveUpView.clickBackTop;
    }
}