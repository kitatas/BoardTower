using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class OfflineModeView : MonoBehaviour
    {
        [SerializeField] private RectTransform footer = default;

        public Tween Show(float duration)
        {
            return DOTween.Sequence()
                .Append(footer
                    .DOAnchorPosY(Mathf.Abs(footer.anchoredPosition.y), duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        public Tween Hide(float duration)
        {
            return DOTween.Sequence()
                .Append(footer
                    .DOAnchorPosY(Mathf.Abs(footer.anchoredPosition.y) * -1, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}