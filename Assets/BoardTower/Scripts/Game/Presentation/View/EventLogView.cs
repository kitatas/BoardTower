using BoardTower.Game.Application;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class EventLogView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI message = default;

        public Tween Show(EventLogMessageVO eventLogMessage)
        {
            var y = message.rectTransform.anchoredPosition.y;
            message.text = eventLogMessage.message;

            return DOTween.Sequence()
                .Append(message
                    .DOFade(1.0f, eventLogMessage.duration)
                    .SetEase(Ease.Linear))
                .Join(message.rectTransform
                    .DOAnchorPosY(y + 25.0f, eventLogMessage.duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        public Tween Hide(EventLogMessageVO eventLogMessage)
        {
            var y = message.rectTransform.anchoredPosition.y;

            return DOTween.Sequence()
                .Append(message
                    .DOFade(0.0f, eventLogMessage.duration)
                    .SetEase(Ease.Linear))
                .Join(message.rectTransform
                    .DOAnchorPosY(y + 25.0f, eventLogMessage.duration)
                    .SetEase(Ease.Linear))
                .Append(message.rectTransform
                    .DOAnchorPosY(y - 25.0f, 0.0f)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}