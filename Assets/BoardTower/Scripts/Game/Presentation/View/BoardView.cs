using System;
using System.Linq;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Utility;
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
                sequence.Join(squareViews[i].FadeIn(duration, BoardHelper.GetDelay(i)));
            }

            return sequence;
        }

        public Tween FadeOutSquareAll(float duration)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                sequence.Join(squareViews[i].FadeOut(duration, BoardHelper.GetDelay(i)));
            }

            return sequence;
        }

        public Tween RenderEventSquareAll(EventVO[] events, float duration)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                sequence.Join(squareViews[i].RenderEvent(events[i].sprite, duration, BoardHelper.GetDelay(i)));
            }

            return sequence;
        }

        public Tween ShowHighlightSquare(HighlightVO[] highlights, float duration)
        {
            var sequence = DOTween.Sequence();
            foreach (var highlight in highlights)
            {
                var tween = highlight.highlight switch
                {
                    HighlightSquareType.Movable => squareViews[highlight.index].ShowHighlight(duration),
                    HighlightSquareType.Default => squareViews[highlight.index].HideHighlight(duration),
                    _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_HIGHLIGHT),
                };
                sequence.Join(tween);
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