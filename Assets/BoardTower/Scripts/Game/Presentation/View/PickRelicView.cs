using System.Collections.Generic;
using System.Linq;
using BoardTower.Game.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class PickRelicView : MonoBehaviour
    {
        [SerializeField] private List<RelicView> pickViews = default;
        [SerializeField] private List<RelicView> detailViews = default;

        public void Render(PickRelicVO pickRelic)
        {
            var pickRelics = pickRelic.relics.ToArray();
            for (int i = 0; i < pickViews.Count; i++)
            {
                if (pickRelics.TryGetValue(i, out var relic))
                {
                    pickViews[i].Render(relic);
                    detailViews[i].Render(relic);
                }
                else
                {
                    pickViews[i].RenderEmpty();
                    detailViews[i].RenderEmpty();
                }
            }
        }
    }
}