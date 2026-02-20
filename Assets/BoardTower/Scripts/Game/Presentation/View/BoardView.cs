using System.Threading;
using BoardTower.Common.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class BoardView : MonoBehaviour
    {
        [SerializeField] private SquareView[] squareViews = default;

        public UniTask FadeInAsync(float duration, CancellationToken token)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                sequence.Join(squareViews[i].FadeIn(duration, i * GetDelay(duration)));
            }

            using var tokenSource = this.BuildLinkedTokenSource(token);
            return sequence
                .SetLink(gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, tokenSource.Token);
        }

        public UniTask FadeOutAsync(float duration, CancellationToken token)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                sequence.Join(squareViews[i].FadeOut(duration, i * GetDelay(duration)));
            }

            using var tokenSource = this.BuildLinkedTokenSource(token);
            return sequence
                .SetLink(gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, tokenSource.Token);
        }

        private static float GetDelay(float duration) => duration / 50.0f;
    }
}