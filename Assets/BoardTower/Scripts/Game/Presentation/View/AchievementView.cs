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

        public void Render(IEnumerable<AchievementProgressVO> achievementProgresses)
        {
            foreach (var achievementProgress in achievementProgresses)
            {
                var view = Instantiate(achievementRecordView, transform);
                view.Render(achievementProgress);
            }
        }
    }
}