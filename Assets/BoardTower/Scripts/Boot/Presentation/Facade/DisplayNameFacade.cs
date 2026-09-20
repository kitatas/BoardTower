using BoardTower.Boot.Presentation.View.Modal;
using R3;

namespace BoardTower.Boot.Presentation.Facade
{
    public sealed class DisplayNameFacade
    {
        private readonly DisplayNameModalView _displayNameModalView;

        public DisplayNameFacade(DisplayNameModalView displayNameModalView)
        {
            _displayNameModalView = displayNameModalView;
        }

        public Observable<string> OnDecisionDisplayName() => _displayNameModalView.decisionDisplayName;
    }
}