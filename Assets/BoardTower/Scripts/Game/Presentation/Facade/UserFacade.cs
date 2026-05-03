using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class UserFacade
    {
        private readonly UidView _uidView;

        public UserFacade(UidView uidView)
        {
            _uidView = uidView;
        }

        public void Render(UserVO user)
        {
            _uidView.Render(user.id);
        }
    }
}