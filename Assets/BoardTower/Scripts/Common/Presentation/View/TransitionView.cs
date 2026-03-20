using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Common.Presentation.View
{
    public sealed class TransitionView : MonoBehaviour
    {
        [SerializeField] private Image mask = default;
        [SerializeField] private Image blocker = default;

        public Tween FadeIn(float duration)
        {
            return DOTween.Sequence()
                .AppendCallback(() => blocker.raycastTarget = true)
                .Append(mask.rectTransform
                    .DOAnchorPosX(0.0f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return DOTween.Sequence()
                .Append(mask.rectTransform
                    .DOAnchorPosX(2200.0f, duration)
                    .SetEase(Ease.Linear))
                .AppendCallback(() => blocker.raycastTarget = false)
                .Append(mask.rectTransform
                    .DOAnchorPosX(-2200.0f, 0.0f)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}