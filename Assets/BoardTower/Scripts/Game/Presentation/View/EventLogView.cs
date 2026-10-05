using BoardTower.Game.Application;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class EventLogView : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera = default;
        [SerializeField] private Transform target = default;
        [SerializeField] private RectTransform view = default;
        [SerializeField] private TextMeshProUGUI message = default;

        private Tween _tween;

        private void Update()
        {
            view.position = mainCamera.WorldToScreenPoint(target.position);
        }

        public Tween Tween(EventLogMessageVO eventLogMessage)
        {
            _tween?.Kill(true);

            var y = message.rectTransform.anchoredPosition.y;
            return _tween = DOTween.Sequence()
                .Append(Show(eventLogMessage, y))
                .AppendInterval(eventLogMessage.duration)
                .Append(Hide(eventLogMessage, y))
                .SetLink(gameObject);
        }

        private Tween Show(EventLogMessageVO eventLogMessage, float y)
        {
            message.text = eventLogMessage.message;

            return DOTween.Sequence()
                .Append(message
                    .DOFade(1.0f, eventLogMessage.duration / 2.0f)
                    .SetEase(Ease.Linear))
                .Join(message.rectTransform
                    .DOAnchorPosY(y + 25.0f, eventLogMessage.duration / 2.0f)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        private Tween Hide(EventLogMessageVO eventLogMessage, float y)
        {
            return DOTween.Sequence()
                .Append(message
                    .DOFade(0.0f, eventLogMessage.duration / 2.0f)
                    .SetEase(Ease.Linear))
                .Join(message.rectTransform
                    .DOAnchorPosY(y + 50.0f, eventLogMessage.duration / 2.0f)
                    .SetEase(Ease.Linear))
                .Append(message.rectTransform
                    .DOAnchorPosY(y, 0.0f)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}