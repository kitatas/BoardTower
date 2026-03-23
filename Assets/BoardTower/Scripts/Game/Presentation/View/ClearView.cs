using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class ClearView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI clear = default;

        private DOTweenTMPAnimator _tmpAnimator;
        private DOTweenTMPAnimator doTweenTMPAnimator => _tmpAnimator ??= new DOTweenTMPAnimator(clear);

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
                    );
            }

            var highlightColor = new Color(1f, 1f, 0.8f);
            var textInfo = clear.textInfo;
            for (int i = 0; i < charCount; i++)
            {
                var interval = i * 0.05f + (charCount * 0.05f);
                var referenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
                var vertexColors = textInfo.meshInfo[referenceIndex].colors32;
                var vertexIndex = textInfo.characterInfo[i].vertexIndex;
                sequence
                    .Join(DOTween.Sequence()
                        .AppendInterval(interval)
                        .Append(doTweenTMPAnimator
                            .DOColorChar(i, highlightColor, 0.15f)
                            .SetEase(Ease.Linear))
                        .AppendCallback(() =>
                        {
                            vertexColors[vertexIndex + 0] = Color.yellow;
                            vertexColors[vertexIndex + 1] = Color.red;
                            vertexColors[vertexIndex + 2] = Color.red;
                            vertexColors[vertexIndex + 3] = Color.yellow;
                            clear.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                        })
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