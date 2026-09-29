using BoardTower.Game.Application;
using Coffee.UIEffects;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View
{
    public sealed class AchievementRecordView : MonoBehaviour
    {
        [SerializeField] private UIEffect uiEffect = default;
        [SerializeField] private Image icon = default;
        [SerializeField] private TextMeshProUGUI content = default;

        public void Render(AchievementContentVO vo)
        {
            var effect = vo.isAchieve ? vo.achievement.rank.ToAchievementEffect() : AchievementConfig.LOCKED;
            uiEffect.gradationColor1 = effect.gradationColor1;
            uiEffect.gradationColor2 = effect.gradationColor2;
            uiEffect.gradationColor3 = effect.gradationColor3;
            uiEffect.gradationColor4 = effect.gradationColor4;
            uiEffect.edgeColor = effect.edgeColor;
            uiEffect.shadowColor = effect.shadowColor;
            uiEffect.gradationIntensity = effect.gradationIntensity;
            uiEffect.edgeWidth = effect.edgeWidth;
            uiEffect.edgeShinyWidth = effect.edgeShinyWidth;

            uiEffect.edgeMode = effect.edgeShiny
                ? EdgeMode.Shiny
                : EdgeMode.None;

            uiEffect.transitionFilter = effect.transitionShiny
                ? TransitionFilter.Shiny
                : TransitionFilter.None;

            if (effect.transitionShiny) PlayTransitionFilter();

            icon.color = vo.isAchieve
                ? Color.white
                : AchievementConfig.UNACHIEVED_COLOR;

            content.text = vo.isAchieve
                ? vo.content
                : AchievementConfig.SECRET_CONTENT;
        }

        private void PlayTransitionFilter()
        {
            uiEffect.transitionRate = -1.0f;

            DOTween.Sequence()
                .Append(DOTween.To(
                        () => uiEffect.transitionRate,
                        x => uiEffect.transitionRate = x,
                        1.0f,
                        1.0f
                    )
                    .SetEase(Ease.InOutCirc))
                .AppendInterval(1.5f)
                .SetLoops(-1, LoopType.Restart)
                .SetLink(gameObject);
        }
    }
}