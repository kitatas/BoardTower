using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View.Button;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class OfflineModeView : MonoBehaviour
    {
        [SerializeField] private RectTransform footer = default;

        public Tween FadeIn(float duration)
        {
            ActivateOfflineButtonAll(false);
            return DOTween.Sequence()
                .Append(footer
                    .DOAnchorPosY(Mathf.Abs(footer.anchoredPosition.y), duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            ActivateOfflineButtonAll(true);
            return DOTween.Sequence()
                .Append(footer
                    .DOAnchorPosY(Mathf.Abs(footer.anchoredPosition.y) * -1, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        private static void ActivateOfflineButtonAll(bool value)
        {
            foreach (var button in FindObjectsByType<GameModalButtonView>(FindObjectsSortMode.None))
            {
                var isAny = OfflineConfig.DEACTIVE_GAME_MODALS.Any(x => x == button.type);
                if (isAny) button.SetInteractable(value);
            }
        }
    }
}