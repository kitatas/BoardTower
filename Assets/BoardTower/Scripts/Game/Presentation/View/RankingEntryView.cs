using BoardTower.Game.Application;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class RankingEntryView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rank = default;
        [SerializeField] private TextMeshProUGUI displayName = default;
        [SerializeField] private TextMeshProUGUI score = default;
        [SerializeField] private GameObject highlight = default;

        public void Render(ScoreRankingEntryVO vo)
        {
            rank.text = vo.rank.ToString();
            displayName.text = vo.displayName;
            score.text = vo.score;
            highlight.SetActive(vo.isSelf);
        }
    }
}