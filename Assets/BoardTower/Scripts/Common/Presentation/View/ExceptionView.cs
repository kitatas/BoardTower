using BoardTower.Common.Presentation.View.Button;
using DG.Tweening;
using R3;
using TMPro;
using UniEx;
using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class ExceptionView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private TextMeshProUGUI message = default;
        [SerializeField] private CommonButtonView commonButtonView = default;

        public Observable<Unit> decision => commonButtonView.click
            .Select(_ => Unit.Default);

        public Tween FadeIn(string value, float duration)
        {
            return DOTween.Sequence()
                .AppendCallback(() =>
                {
                    message.text = value;
                    canvasGroup.blocksRaycasts = true;
                })
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
            return DOTween.Sequence()
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