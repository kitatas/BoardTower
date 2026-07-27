using System.Collections.Generic;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class AchievementUseCase
    {
        private readonly MasterEntity _masterEntity;
        private readonly UserEntity _userEntity;
        private readonly AchievementPorts _achievementPorts;

        public AchievementUseCase(MasterEntity masterEntity, UserEntity userEntity, AchievementPorts achievementPorts)
        {
            _masterEntity = masterEntity;
            _userEntity = userEntity;
            _achievementPorts = achievementPorts;
        }

        public IAsyncSubscriber<IEnumerable<AchievementContentVO>> achievementContents =>
            _achievementPorts.achievementContentsSubscriber;

        public UniTask PublishAchievementContentsAsync(CancellationToken token)
        {
            var vos = new List<AchievementContentVO>();
            foreach (var achievement in _masterEntity.achievements)
            {
                var progress = _userEntity.Find(achievement.type);
                vos.Add(new AchievementContentVO(achievement, progress));
            }

            return _achievementPorts.PublishAchievementContentsAsync(vos, token);
        }
    }
}