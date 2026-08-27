using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class GameModeView : MonoBehaviour
    {
        [SerializeField] private OfflineModeView offlineModeView = default;

        public Tween ShowOnline(float duration)
        {
            return DOTween.Sequence()
                .Append(offlineModeView.Hide(duration))
                .SetLink(gameObject);
        }

        public Tween ShowOffline(float duration)
        {
            return DOTween.Sequence()
                .Append(offlineModeView.Show(duration))
                .SetLink(gameObject);
        }
    }
}