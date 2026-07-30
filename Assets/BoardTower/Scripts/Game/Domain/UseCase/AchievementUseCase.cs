using System.Collections.Generic;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Repository;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class AchievementUseCase
    {
        private readonly UserEntity _userEntity;
        private readonly AchievementPorts _achievementPorts;
        private readonly LocaleRepository _localeRepository;

        public AchievementUseCase(UserEntity userEntity, AchievementPorts achievementPorts,
            LocaleRepository localeRepository)
        {
            _userEntity = userEntity;
            _achievementPorts = achievementPorts;
            _localeRepository = localeRepository;
        }

        public IAsyncSubscriber<IEnumerable<AchievementContentVO>> achievementContents =>
            _achievementPorts.achievementContentsSubscriber;

        public UniTask PublishAchievementContentsAsync(CancellationToken token)
        {
            var vos = new List<AchievementContentVO>();
            // foreach (var achievement in _masterEntity.achievements)
            // {
            //     var progress = _userEntity.Find(achievement.type);
            //     var isAchieve = achievement.value <= progress.value;
            //     var content = GetContent(achievement);
            //     vos.Add(new AchievementContentVO(achievement, isAchieve, content));
            // }

            return _achievementPorts.PublishAchievementContentsAsync(vos, token);
        }

        private string GetContent(AchievementVO achievement)
        {
            var key = ZString.Format(LocaleConfig.ACHIEVEMENT_CONTENT_KEY, achievement.type);
            return ZString.Format(_localeRepository.Get(key), achievement.value);
        }
    }
}