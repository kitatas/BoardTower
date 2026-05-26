using DG.Tweening;
using UnityEngine;

namespace BoardTower.Boot.Presentation.View
{
    public sealed class DisplayNameView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;

        private Tween _tween;

        public Tween FadeIn(float duration)
        {
            _tween?.Kill(true);

            return _tween = DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.Linear))
                .AppendCallback(() => canvasGroup.blocksRaycasts = true)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            _tween?.Kill(true);

            return _tween = DOTween.Sequence()
                .AppendCallback(() => canvasGroup.blocksRaycasts = false)
                .Append(canvasGroup
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}