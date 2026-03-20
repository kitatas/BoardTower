using BoardTower.Game.Application;
using DG.Tweening;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SquareEventBlockView : MonoBehaviour
    {
        [SerializeField] private Transform spike = default;

        private void Awake()
        {
            spike
                .SetLocalPositionY(0.0f)
                .DOLocalMoveY(-0.15f, BoardConfig.EVENT_OBJECT_DURATION / 4.0f)
                .SetEase(Ease.InCirc)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(spike.gameObject);
        }
    }
}