using System.Collections.Generic;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class AchievementFacade
    {
        private readonly AchievementView _achievementView;

        public AchievementFacade(AchievementView achievementView)
        {
            _achievementView = achievementView;
        }

        public UniTask RenderAsync(IEnumerable<AchievementContentVO> achievementContents, CancellationToken token)
        {
            _achievementView.Refresh();
            _achievementView.Render(achievementContents);
            return UniTask.Yield(token);
        }
    }
}