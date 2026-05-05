using System.Collections.Generic;
using System.Linq;
using BoardTower.Game.Application;
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
    }
}