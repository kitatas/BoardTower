using System;
using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;

namespace BoardTower.Common.Data.Entity
{
    public sealed class UserEntity : BaseEntity<UserVO>
    {
        public string displayName => value.playFabUser.displayName.value;
        public bool isRegistered => !string.IsNullOrEmpty(displayName);

        private Dictionary<AchievementType, ProgressVO> _achievement;

        private Dictionary<AchievementType, ProgressVO> achievement =>
            _achievement ??= (value.playFabUser.progresses ?? Array.Empty<ProgressVO>())
                .ToDictionary(x => x.type, x => x);

        public void SetDisplayName(UserDisplayNameVO userDisplayName)
        {
            var localUser = value.localUser;
            var playFabUser = new PlayFabUserVO(value.playFabUser.isNewly, userDisplayName, value.playFabUser.progresses);
            var user = new UserVO(localUser, playFabUser);
            Set(user);
        }

        public ProgressVO Find(AchievementType type)
        {
            if (achievement.TryGetValue(type, out var progress)) return progress;
            return new ProgressVO(type, 0);
        }
    }
}