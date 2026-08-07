using BoardTower.Game.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class ScoreRankingView : MonoBehaviour
    {
        [SerializeField] private RankingEntryView rankingEntryView = default;

        public void Refresh()
        {
            gameObject.DestroyChildren();
        }

        public void Render(ScoreRankingVO scoreRanking)
        {
            foreach (var entry in scoreRanking.entries)
            {
                var view = Instantiate(rankingEntryView, transform);
                view.Render(entry);
            }
        }
    }
}