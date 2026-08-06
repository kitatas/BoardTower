using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;
using PlayFab.ProgressionModels;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class PlayFabRankingDTO
    {
        private readonly List<EntityLeaderboardEntry> _rankings;

        public PlayFabRankingDTO(List<EntityLeaderboardEntry> rankings)
        {
            _rankings = rankings;
        }

        public PlayFabRankingVO ToVO() => new(_rankings
            .Select(x => new PlayFabRankingEntryVO(x.Entity.Id, x.Rank, x.DisplayName, x.Scores[0]))
        );
    }
}