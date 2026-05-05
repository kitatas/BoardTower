using System.Collections.Generic;
using System.Linq;
using BoardTower.Game.Application;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class LotRelicView : MonoBehaviour
    {
        [SerializeField] private List<RelicView> relicViews = default;

        public void Render(LotRelicVO lotRelic)
        {
            for (int i = 0; i < relicViews.Count; i++)
            {
                relicViews[i].Render(lotRelic.relics.ElementAt(i));
            }
        }

        public Tween FadeIn(float duration)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < relicViews.Count; i++)
            {
                var delay = duration > 0.0f ? i * RelicConfig.LOT_DELAY_RATE : 0.0f;
                sequence.Join(relicViews[i].FadeIn(duration, delay));
            }

            return sequence;
        }

        public Tween FadeOut(float duration)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < relicViews.Count; i++)
            {
                var delay = duration > 0.0f ? i * RelicConfig.LOT_DELAY_RATE : 0.0f;
                sequence.Join(relicViews[i].FadeOut(duration, delay));
            }

            return sequence;
        }
    }
}