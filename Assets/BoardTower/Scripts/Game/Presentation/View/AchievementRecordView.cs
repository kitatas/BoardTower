using BoardTower.Game.Application;
using Coffee.UIEffects;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class AchievementRecordView : MonoBehaviour
    {
        [SerializeField] private UIEffect uiEffect = default;
        [SerializeField] private TextMeshProUGUI content = default;

        public void Render(AchievementContentVO vo)
        {
            var effect = vo.achievement.rank.ToAchievementEffect();
            uiEffect.gradationColor1 = effect.gradationColor1;
            uiEffect.gradationColor2 = effect.gradationColor2;
            uiEffect.gradationColor3 = effect.gradationColor3;
            uiEffect.gradationColor4 = effect.gradationColor4;
            uiEffect.edgeColor = effect.edgeColor;
            uiEffect.shadowColor = effect.shadowColor;
            uiEffect.gradationIntensity = effect.gradationIntensity;
            uiEffect.edgeWidth = effect.edgeWidth;
            uiEffect.edgeShinyWidth = effect.edgeShinyWidth;
            uiEffect.edgeShinyRate = effect.edgeShinyRate;

            content.text = vo.isAchieve
                ? vo.content
                : AchievementConfig.SECRET_CONTENT;
        }
    }
}