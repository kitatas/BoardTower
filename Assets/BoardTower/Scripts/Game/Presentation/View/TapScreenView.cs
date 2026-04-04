using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class TapScreenView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;

        public Tween FadeIn(float duration)
        {
            return DOTween.Sequence()
                .AppendCallback(() => canvasGroup.blocksRaycasts = true)
                .Append(canvasGroup
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.OutBack))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.OutQuart))
                .AppendCallback(() => canvasGroup.blocksRaycasts = false)
                .SetLink(gameObject);
        }
    }
}