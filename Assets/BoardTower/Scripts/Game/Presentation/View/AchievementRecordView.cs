using BoardTower.Common.Application;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class AchievementRecordView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI content = default;

        public void Render(AchievementContentVO vo)
        {
            // TODO: set icon
            content.text = vo.isAchieve
                ? vo.content
                : AchievementConfig.SECRET_CONTENT;
        }
    }
}