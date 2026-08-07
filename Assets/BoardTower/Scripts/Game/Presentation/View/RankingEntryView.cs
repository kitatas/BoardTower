using BoardTower.Common.Application;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class RankingEntryView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rank = default;
        [SerializeField] private TextMeshProUGUI displayName = default;
        [SerializeField] private TextMeshProUGUI score = default;

        public void Render(PlayFabRankingEntryVO vo)
        {
            rank.text = vo.rank.ToString();
            displayName.text = vo.displayName;
            score.text = vo.score;
        }
    }
}