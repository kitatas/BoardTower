using System;
using System.Linq;
using DG.Tweening;
using R3;
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

        public Tween ShowHighlightSquare(int[] indices, float duration)
        {
            var sequence = DOTween.Sequence();
            foreach (var i in indices)
            {
                sequence.Join(squareViews[i].ShowHighlight(duration));
            }

            return sequence;
        }

        public Observable<int> OnClickAnySquareAsObservable()
        {
            return squareViews
                .Select((sv, i) =>
                    Observable.FromEvent<Action, Unit>(
                        x => () => x(Unit.Default),
                        x => sv.click += x,
                        x => sv.click -= x
                    ).Select(_ => i))
                .Merge()
                .Share();
        }
    }
}