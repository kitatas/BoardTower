using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class LotRelicNoticeView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;

        public Tween FadeIn(float duration)
        {
            return canvasGroup
                .DOFade(1.0f, duration)
                .SetEase(Ease.OutBack)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return canvasGroup
                .DOFade(0.0f, duration)
                .SetEase(Ease.OutQuart)
                .SetLink(gameObject);
        }
    }
}