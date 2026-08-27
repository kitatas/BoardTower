using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class OfflineModeView : MonoBehaviour
    {
        [SerializeField] private RectTransform footer = default;

        public Tween FadeIn(float duration)
        {
            return DOTween.Sequence()
                .Append(footer
                    .DOAnchorPosY(Mathf.Abs(footer.anchoredPosition.y), duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return DOTween.Sequence()
                .Append(footer
                    .DOAnchorPosY(Mathf.Abs(footer.anchoredPosition.y) * -1, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}