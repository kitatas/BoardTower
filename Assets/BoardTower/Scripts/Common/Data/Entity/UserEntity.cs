using BoardTower.Common.Application;

namespace BoardTower.Common.Data.Entity
{
    public sealed class UserEntity : BaseEntity<UserVO>
    {
        public string displayName => value.playFabUser.displayName.value;
        public bool isRegistered => string.IsNullOrEmpty(displayName);
    }
}