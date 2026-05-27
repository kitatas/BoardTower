using BoardTower.Common.Presentation.View.Button;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;

namespace BoardTower.Boot.Presentation.View
{
    public sealed class DisplayNameView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private TMP_InputField inputField = default;
        [SerializeField] private CommonButtonView commonButtonView = default;

        private Tween _tween;

        public Observable<string> decisionDisplayName => commonButtonView.click
            .Select(_ => inputField.text);

        public Tween FadeIn(float duration)
        {
            _tween?.Kill(true);

            return _tween = DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.Linear))
                .AppendCallback(() => canvasGroup.blocksRaycasts = true)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            _tween?.Kill(true);

            return _tween = DOTween.Sequence()
                .AppendCallback(() => canvasGroup.blocksRaycasts = false)
                .Append(canvasGroup
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}