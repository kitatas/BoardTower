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

        public async UniTask FadeInAsync(float duration, CancellationToken token)
        {
            using var tokenSource = this.BuildLinkedTokenSource(token);

            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                sequence.Join(squareViews[i].FadeIn(duration, i * 0.01f));
            }

            await sequence
                .SetLink(gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, tokenSource.Token);
        }

        public async UniTask FadeOutAsync(float duration, CancellationToken token)
        {
            using var tokenSource = this.BuildLinkedTokenSource(token);

            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                sequence.Join(squareViews[i].FadeOut(duration, i * 0.01f));
            }

            await sequence
                .SetLink(gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, tokenSource.Token);
        }
    }
}