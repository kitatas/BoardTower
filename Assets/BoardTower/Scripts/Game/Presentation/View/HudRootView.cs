using BoardTower.Game.Application;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class HudRootView : MonoBehaviour
    {
        [SerializeField] private RectTransform[] huds = default;

        public Tween FadeIn(float duration)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < huds.Length; i++)
            {
                var x = Mathf.Abs(huds[i].anchoredPosition.x);
                sequence
                    .Join(huds[i]
                        .DOAnchorPosX(x, duration)
                        .SetEase(Ease.OutBack)
                        .SetDelay(duration * i * HudRootConfig.FADE_DELAY_RATE))
                    .SetLink(gameObject);
            }

            return sequence;
        }

        public Tween FadeOut(float duration)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < huds.Length; i++)
            {
                var x = Mathf.Abs(huds[i].anchoredPosition.x);
                sequence
                    .Join(huds[i]
                        .DOAnchorPosX(x * -1.0f, duration)
                        .SetEase(Ease.OutBack)
                        .SetDelay(duration * i * HudRootConfig.FADE_DELAY_RATE))
                    .SetLink(gameObject);
            }

            return sequence;
        }
    }
}