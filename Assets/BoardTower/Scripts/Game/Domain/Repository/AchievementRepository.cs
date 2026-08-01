using System.Collections.Generic;
using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.DataStore.Tables;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class AchievementRepository
    {
        private readonly AchievementMasterTable _achievementMasterTable;

        public AchievementRepository(MemoryDbData memoryDatabase)
        {
            _achievementMasterTable = memoryDatabase.memoryDatabase.AchievementMasterTable;
        }

        public IEnumerable<AchievementVO> GetAll()
        {
            return _achievementMasterTable.All
                .Select(x => x.ToVO());
        }
    }
}