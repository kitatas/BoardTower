using BoardTower.Game.Application;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class ChessmenView : MonoBehaviour
    {
        public Tween FadeIn(float duration)
        {
            return transform
                .DOLocalMoveY(3.0f, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return transform
                .DOLocalMoveY(12.0f, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public Tween Move(SquareVO square, float duration)
        {
            return DOTween.Sequence()
                .Append(transform
                    .DOLocalMoveX(square.localX, duration))
                .Join(transform
                    .DOLocalMoveZ(square.localZ, duration))
                .SetLink(gameObject);
        }
    }
}