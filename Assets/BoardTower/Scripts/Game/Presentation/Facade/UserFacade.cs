using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class UserFacade
    {
        private readonly DisplayNameView _displayNameView;
        private readonly UidView _uidView;

        public UserFacade(DisplayNameView displayNameView, UidView uidView)
        {
            _displayNameView = displayNameView;
            _uidView = uidView;
        }

        public void Render(UserVO user)
        {
            _displayNameView.Render(user.playFabUser.displayName.value);
            _uidView.Render(user.localUser.id);
        }
    }
}