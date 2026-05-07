using System.Collections.Generic;
using System.Linq;
using BoardTower.Game.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class PickRelicView : MonoBehaviour
    {
        [SerializeField] private List<RelicView> relicViews = default;

        public void Render(PickRelicVO pickRelic)
        {
            var pickRelics = pickRelic.relics.ToArray();
            for (int i = 0; i < relicViews.Count; i++)
            {
                if (pickRelics.TryGetValue(i, out var relic))
                {
                    relicViews[i].Render(relic);
                }
                else
                {
                    relicViews[i].RenderEmpty();
                }
            }
        }
    }
}