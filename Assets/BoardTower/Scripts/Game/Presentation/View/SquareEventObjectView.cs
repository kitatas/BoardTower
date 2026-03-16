using BoardTower.Common.Application;
using BoardTower.Game.Application;
using DG.Tweening;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SquareEventObjectView : MonoBehaviour
    {
        [SerializeField] private Transform eventObjectTransform = default;

        private void Awake()
        {
            eventObjectTransform
                .DORotate(new Vector3(0.0f, 360.0f, 0.0f), BoardConfig.EVENT_OBJECT_DURATION, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutCirc)
                .SetLoops(-1, LoopType.Restart)
                .SetLink(eventObjectTransform.gameObject);
        }

        public Tween Render(GameObject eventObject, float duration, float delay)
        {
            eventObjectTransform.gameObject.DestroyChildren();
            if (eventObject == null) return null;

            var _ = Instantiate(eventObject, eventObjectTransform);
            return DOTween.Sequence()
                .Append(eventObjectTransform
                    .DOLocalMoveY(-1.0f, 0.0f))
                .Append(eventObjectTransform
                    .DOLocalMoveY(0.2f, duration))
                .SetDelay(delay)
                .SetLink(eventObjectTransform.gameObject);
        }
    }
}