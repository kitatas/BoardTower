using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;
using R3;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class AccountFacade
    {
        private readonly DisplayNameView _displayNameView;
        private readonly UidView _uidView;

        public AccountFacade(DisplayNameView displayNameView, UidView uidView)
        {
            _displayNameView = displayNameView;
            _uidView = uidView;
        }

        public Observable<string> OnDecisionDisplayName() => _displayNameView.decisionDisplayName;

        public void Render(UserVO user)
        {
            _displayNameView.Render(user.playFabUser.displayName.value);
            _uidView.Render(user.localUser.id);
        }
    }
}