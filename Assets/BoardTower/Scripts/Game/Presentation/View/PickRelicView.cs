using System.Collections.Generic;
using System.Linq;
using BoardTower.Game.Application;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class PickRelicView : MonoBehaviour
    {
        [SerializeField] private List<RelicView> relicViews = default;

        public void Render(PickRelicVO pickRelic)
        {
            var pickRelics = pickRelic.relics.ToArray();
            for (int i = 0; i < pickRelics.Length; i++)
            {
                relicViews[i].Render(pickRelics[i]);
            }
        }
    }
}