using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SquareView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform highlight = default;
        [SerializeField] private SpriteRenderer squareEvent = default;

        public event Action click;

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

        public Tween ShowHighlight(float duration)
        {
            return DOTween.Sequence()
                .Append(highlight
                    .DOLocalMoveY(0.05f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        public Tween HideHighlight(float duration)
        {
            return DOTween.Sequence()
                .Append(highlight
                    .DOLocalMoveY(0.0f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData _)
        {
            click?.Invoke();
        }

        public Tween RenderEvent(Sprite sprite, float duration, float delay)
        {
            if (squareEvent.sprite == sprite) return null;

            return DOTween.Sequence()
                .Append(squareEvent.transform
                    .DOLocalMoveY(0.49f, 0.0f))
                .AppendCallback(() => squareEvent.sprite = sprite)
                .Append(squareEvent.transform
                    .DOLocalMoveY(0.51f, duration))
                .SetDelay(delay)
                .SetLink(gameObject);
        }
    }
}