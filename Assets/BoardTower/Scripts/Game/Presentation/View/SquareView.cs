using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SquareView : MonoBehaviour
    {
        [SerializeField] private Transform highlight = default;

        public Tween FadeIn(float duration, float delay)
        {
            return DOTween.Sequence()
                .Append(transform
                    .DOLocalMoveY(0.0f, duration)
                    .SetEase(Ease.OutBack))
                .SetDelay(delay)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration, float delay)
        {
            return DOTween.Sequence()
                .Append(transform
                    .DOLocalMoveY(-2.5f, duration)
                    .SetEase(Ease.OutQuart))
                .SetDelay(delay)
                .SetLink(gameObject);
        }

        public Tween ShowHighlight(float duration)
        {
            return DOTween.Sequence()
                .Append(highlight
                    .DOLocalMoveY(0.05f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}