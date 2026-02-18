using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SquareView : MonoBehaviour
    {
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
    }
}