using BoardTower.Common.Application;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class GameModeView : MonoBehaviour
    {
        [SerializeField] private OfflineModeView offlineModeView = default;

        public Tween FadeIn(GameMode mode, float duration)
        {
            return mode switch
            {
                GameMode.Online => FadeInOnline(duration),
                GameMode.Offline => FadeInOffline(duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_GAME_MODE),
            };
        }

        private Tween FadeInOnline(float duration)
        {
            return DOTween.Sequence()
                .Append(offlineModeView.FadeOut(duration))
                .SetLink(gameObject);
        }

        private Tween FadeInOffline(float duration)
        {
            return DOTween.Sequence()
                .Append(offlineModeView.FadeIn(duration))
                .SetLink(gameObject);
        }
    }
}