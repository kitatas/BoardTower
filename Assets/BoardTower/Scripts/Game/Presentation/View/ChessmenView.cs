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
                .Append(MoveXZ(square, duration))
                .Join(MoveY(duration))
                .SetLink(gameObject);
        }

        private Tween MoveXZ(SquareVO square, float duration)
        {
            return DOTween.Sequence()
                .Append(transform
                    .DOLocalMoveX(square.localX, duration)
                    .SetEase(Ease.Linear))
                .Join(transform
                    .DOLocalMoveZ(square.localZ, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        private Tween MoveY(float duration)
        {
            return DOTween.Sequence()
                .Append(transform
                    .DOLocalMoveY(4.5f, duration / 2.0f)
                    .SetEase(Ease.OutQuart))
                .Append(transform
                    .DOLocalMoveY(3.0f, duration / 2.0f)
                    .SetEase(Ease.InQuart))
                .SetLink(gameObject);
        }
    }
}