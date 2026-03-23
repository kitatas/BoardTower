using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class FailView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fail = default;

        private DOTweenTMPAnimator _tmpAnimator;
        private DOTweenTMPAnimator doTweenTMPAnimator => _tmpAnimator ??= new DOTweenTMPAnimator(fail);

        public Tween FadeIn(float duration)
        {
            var charCount = doTweenTMPAnimator.textInfo.characterCount;
            var offset = Vector3.up * 40.0f;

            var sequence = DOTween.Sequence();
            for (int i = 0; i < charCount; i++)
            {
                sequence
                    .Join(DOTween.Sequence()
                        .Append(doTweenTMPAnimator
                            .DOOffsetChar(i, doTweenTMPAnimator.GetCharOffset(i) + offset, duration)
                            .SetEase(Ease.OutFlash, 2))
                        .Join(doTweenTMPAnimator
                            .DOFadeChar(i, 1.0f, duration))
                        .SetDelay(i * 0.04f)
                        .SetLink(gameObject)
                    );
            }

            return sequence;
        }

        public Tween FadeOut(float duration)
        {
            var charCount = doTweenTMPAnimator.textInfo.characterCount;

            var sequence = DOTween.Sequence();
            for (int i = 0; i < charCount; i++)
            {
                sequence
                    .Join(doTweenTMPAnimator
                        .DOFadeChar(i, 0.0f, duration))
                    .SetLink(gameObject);
            }

            return sequence;
        }
    }
}