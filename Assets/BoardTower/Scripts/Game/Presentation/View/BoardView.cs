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
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                // NOTE: warning CS4014 の解消
                var _ = sequence.Join(squareViews[i].FadeIn(duration, i * GetDelay(duration)));
            }

            using var tokenSource = this.BuildLinkedTokenSource(token);
            await sequence
                .SetLink(gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, tokenSource.Token);
        }

        public async UniTask FadeOutAsync(float duration, CancellationToken token)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < squareViews.Length; i++)
            {
                // NOTE: warning CS4014 の解消
                var _ = sequence.Join(squareViews[i].FadeOut(duration, i * GetDelay(duration)));
            }

            using var tokenSource = this.BuildLinkedTokenSource(token);
            await sequence
                .SetLink(gameObject)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, tokenSource.Token);
        }

        private static float GetDelay(float duration) => duration / 50.0f;
    }
}