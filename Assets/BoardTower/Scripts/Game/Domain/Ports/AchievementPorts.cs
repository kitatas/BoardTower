using System.Collections.Generic;
using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class AchievementPorts
    {
        public readonly IAsyncSubscriber<IEnumerable<AchievementContentVO>> achievementContentsSubscriber;
        private readonly IAsyncPublisher<IEnumerable<AchievementContentVO>> _achievementContentsPublisher;

        public AchievementPorts(IAsyncSubscriber<IEnumerable<AchievementContentVO>> achievementContentsSubscriber,
            IAsyncPublisher<IEnumerable<AchievementContentVO>> achievementContentsPublisher)
        {
            this.achievementContentsSubscriber = achievementContentsSubscriber;
            _achievementContentsPublisher = achievementContentsPublisher;
        }

        public UniTask PublishAchievementContentsAsync(IEnumerable<AchievementContentVO> achievementContents, CancellationToken token)
        {
            return _achievementContentsPublisher.PublishAsync(achievementContents, token);
        }
    }
}