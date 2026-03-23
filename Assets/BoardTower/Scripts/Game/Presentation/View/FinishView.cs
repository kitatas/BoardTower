using BoardTower.Common.Application;
using BoardTower.Game.Application;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class FinishView : MonoBehaviour
    {
        [SerializeField] private ClearView clearView = default;
        [SerializeField] private FailView failView = default;

        public Tween FadeIn(FinishType type, float duration)
        {
            return type switch
            {
                FinishType.Clear => clearView.FadeIn(duration),
                FinishType.Fail => failView.FadeIn(duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FINISH),
            };
        }

        public Tween FadeOut(FinishType type, float duration)
        {
            return type switch
            {
                FinishType.Clear => clearView.FadeOut(duration),
                FinishType.Fail => failView.FadeOut(duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FINISH),
            };
        }
    }
}