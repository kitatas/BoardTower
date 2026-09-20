using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class GameModeView : MonoBehaviour
    {
        [SerializeField] private OfflineModeView offlineModeView = default;

        public Tween FadeInOnline(float duration)
        {
            return DOTween.Sequence()
                .Append(offlineModeView.FadeOut(duration))
                .SetLink(gameObject);
        }

        public Tween FadeInOffline(float duration)
        {
            return DOTween.Sequence()
                .Append(offlineModeView.FadeIn(duration))
                .SetLink(gameObject);
        }
    }
}