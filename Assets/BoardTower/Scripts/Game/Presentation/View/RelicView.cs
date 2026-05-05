using BoardTower.Game.Application;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class RelicView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private TextMeshProUGUI content = default;

        public void Render(RelicVO value)
        {
            content.text = value.content;
        }

        public Tween FadeIn(float duration, float delay)
        {
            return canvasGroup
                .DOFade(1.0f, duration)
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration, float delay)
        {
            return canvasGroup
                .DOFade(0.0f, duration)
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .SetLink(gameObject);
        }
    }
}