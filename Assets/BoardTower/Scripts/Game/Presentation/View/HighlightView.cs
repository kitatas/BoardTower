using BoardTower.Game.Application;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class HighlightView : MonoBehaviour
    {
        private void Awake()
        {
            var scale = new Vector3(0.9f, 1.0f, 0.9f);
            var rate = new Vector3(1.2f, 1.0f, 1.2f);
            transform
                .DOScale(Vector3.Scale(scale, rate), BoardConfig.EVENT_OBJECT_DURATION / 4.0f)
                .SetEase(Ease.OutQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }
    }
}