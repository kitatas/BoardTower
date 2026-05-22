using BoardTower.Common.Utility;
using BoardTower.Game.Application;
using Cysharp.Text;
using DG.Tweening;
using FastEnumUtility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View
{
    public sealed class RelicView : UIBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private Image icon = default;
        [SerializeField] private TextMeshProUGUI relicName = default;
        [SerializeField] private TextMeshProUGUI content = default;
        [SerializeField] private GameObject uniqLabel = default;

        public void Render(RelicVO value)
        {
            var path = ZString.Format("Assets/Externals/Sprites/UI/relic.png[relic_{0}]", value.type.ToInt32());
            this.LoadAsset<Sprite>(path, x =>
            {
                icon.sprite = x;
                relicName.text = ZString.Format("{0}", value.relicName);
                content.text = ZString.Format("{0}", value.content);
                uniqLabel.SetActive(value.isUniq);
            });
        }

        public void RenderEmpty()
        {
            icon.sprite = null;
            relicName.text = ZString.Format("{0}", "---");
            content.text = ZString.Format("{0}", "---");
            uniqLabel.SetActive(false);
        }

        public Tween FadeIn(float duration, float delay)
        {
            return DOTween.Sequence()
                .AppendCallback(() => canvasGroup.blocksRaycasts = true)
                .Append(canvasGroup
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.Linear))
                .SetDelay(delay)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration, float delay)
        {
            return DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.Linear))
                .AppendCallback(() => canvasGroup.blocksRaycasts = false)
                .SetDelay(delay)
                .SetLink(gameObject);
        }
    }
}