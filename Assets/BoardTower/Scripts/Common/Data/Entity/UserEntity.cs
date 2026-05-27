using BoardTower.Common.Application;

namespace BoardTower.Common.Data.Entity
{
    public sealed class UserEntity : BaseEntity<UserVO>
    {
        public string displayName => value.playFabUser.displayName.value;
        public bool isRegistered => !string.IsNullOrEmpty(displayName);

        public void SetDisplayName(UserDisplayNameVO userDisplayName)
        {
            var localUser = value.localUser;
            var playFabUser = new PlayFabUserVO(value.playFabUser.isNewly, userDisplayName);
            var user = new UserVO(localUser, playFabUser);
            Set(user);
        }
    }
}