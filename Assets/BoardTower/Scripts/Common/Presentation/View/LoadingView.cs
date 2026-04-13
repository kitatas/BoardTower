using DG.Tweening;
using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class LoadingView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;

        public Tween FadeIn(float duration)
        {
            return Fade(1.0f, duration);
        }

        public Tween FadeOut(float duration)
        {
            return Fade(0.0f, duration);
        }

        private Tween Fade(float value, float duration)
        {
            return DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(value, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}