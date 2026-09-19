using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View.Button;
using DG.Tweening;
using R3;
using UniEx;
using UnityEngine;

namespace BoardTower.Boot.Presentation.View
{
    public sealed class GameModeView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private CommonButtonView commonButtonView = default;

        private Tween _tween;

        public Observable<SeType> decision => commonButtonView.click;

        public Tween FadeIn(float duration)
        {
            _tween?.Kill(true);

            return _tween = DOTween.Sequence()
                .AppendCallback(() => canvasGroup.blocksRaycasts = true)
                .Append(canvasGroup
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.OutBack))
                .Join(canvasGroup.transform.ToRectTransform()
                    .DOScale(Vector3.one, duration)
                    .SetEase(Ease.OutBack))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            _tween?.Kill(true);

            return _tween = DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.OutQuart))
                .Join(canvasGroup.transform.ToRectTransform()
                    .DOScale(Vector3.one * 0.8f, duration)
                    .SetEase(Ease.OutQuart))
                .AppendCallback(() => canvasGroup.blocksRaycasts = false)
                .SetLink(gameObject);
        }
    }
}