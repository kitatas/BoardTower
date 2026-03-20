using BoardTower.Game.Application;
using DG.Tweening;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SquareEventBeltView : MonoBehaviour
    {
        [SerializeField] private Transform arrow = default;

        private void Awake()
        {
            arrow
                .SetLocalPositionX(0.15f)
                .DOLocalMoveX(-0.15f, BoardConfig.EVENT_OBJECT_DURATION / 4.0f)
                .SetEase(Ease.InCubic)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(arrow.gameObject);
        }
    }
}