using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class BoardView : MonoBehaviour
    {
        [SerializeField] private SquareView[] squareViews = default;

        public Tween FadeInSquareAll(float duration)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                sequence.Join(squareViews[i].FadeIn(duration, i * GetDelay(duration)));
            }

            return sequence;
        }

        public Tween FadeOutSquareAll(float duration)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                sequence.Join(squareViews[i].FadeOut(duration, i * GetDelay(duration)));
            }

            return sequence;
        }

        private static float GetDelay(float duration) => duration / 50.0f;
    }
}