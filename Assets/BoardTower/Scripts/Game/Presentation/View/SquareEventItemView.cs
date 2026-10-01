using BoardTower.Game.Application;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SquareEventItemView : MonoBehaviour
    {
        private void Awake()
        {
            var y = transform.localRotation.eulerAngles.y;
            transform
                .DORotate(new Vector3(0.0f, y + 360.0f, 0.0f), BoardConfig.EVENT_OBJECT_DURATION, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutCirc)
                .SetLoops(-1, LoopType.Restart)
                .SetLink(gameObject);
        }
    }
}