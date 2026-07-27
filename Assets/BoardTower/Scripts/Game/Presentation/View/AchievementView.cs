using System.Collections.Generic;
using BoardTower.Common.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class AchievementView : MonoBehaviour
    {
        [SerializeField] private AchievementRecordView achievementRecordView = default;

        public void Refresh()
        {
            gameObject.DestroyChildren();
        }

        public void Render(IEnumerable<AchievementContentVO> achievementContents)
        {
            foreach (var achievementProgress in achievementContents)
            {
                var view = Instantiate(achievementRecordView, transform);
                view.Render(achievementProgress);
            }
        }
    }
}