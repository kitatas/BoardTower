using System.Collections.Generic;
using System.Threading;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class AchievementPorts
    {
        public readonly IAsyncSubscriber<IEnumerable<AchievementProgressVO>> achievementProgressesSubscriber;
        private readonly IAsyncPublisher<IEnumerable<AchievementProgressVO>> _achievementProgressesPublisher;

        public AchievementPorts(IAsyncSubscriber<IEnumerable<AchievementProgressVO>> achievementProgressesSubscriber,
            IAsyncPublisher<IEnumerable<AchievementProgressVO>> achievementProgressesPublisher)
        {
            this.achievementProgressesSubscriber = achievementProgressesSubscriber;
            _achievementProgressesPublisher = achievementProgressesPublisher;
        }

        public UniTask PublishAchievementProgressAsync(IEnumerable<AchievementProgressVO> achievementProgresses, CancellationToken token)
        {
            return _achievementProgressesPublisher.PublishAsync(achievementProgresses, token);
        }
    }
}