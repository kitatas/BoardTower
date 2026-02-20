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
    }
}