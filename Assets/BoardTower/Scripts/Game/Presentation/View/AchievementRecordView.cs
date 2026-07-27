using BoardTower.Common.Application;
using Cysharp.Text;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class AchievementRecordView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI content = default;

        public void Render(AchievementContentVO vo)
        {
            // TODO: locale
            var isAchieve = vo.achievement.value <= vo.progress.value;
            var key = ZString.Format("{0}_{1}", vo.achievement.type, vo.achievement.rank);
            content.text = isAchieve
                ? ZString.Format("{0}_{1}: {2}", key, vo.achievement.value, isAchieve)
                : ZString.Format("{0}_{1}: ???({2})", key, vo.achievement.value, isAchieve);
        }
    }
}