using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Boot.Presentation.View
{
    public sealed class SplashView : MonoBehaviour
    {
        [SerializeField] private Image copyright = default;

        private Tween _tween;

        public Tween FadeIn(Sprite sprite, float duration)
        {
            Kill();

            copyright.sprite = sprite;
            _tween = DOTween.Sequence()
                .Append(copyright
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);

            return _tween;
        }

        public Tween FadeOut(float duration)
        {
            Kill();

            _tween = DOTween.Sequence()
                .Append(copyright
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);

            return _tween;
        }

        public void Kill()
        {
            _tween?.Kill(true);
        }
    }
}