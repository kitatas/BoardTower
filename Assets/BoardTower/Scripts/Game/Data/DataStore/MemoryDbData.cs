using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;

namespace BoardTower.Game.Data.DataStore
{
    public sealed class MemoryDbData
    {
        public readonly MemoryDatabase memoryDatabase;

        public MemoryDbData(MemoryDatabase memoryDatabase, PlayFabTitleData playFabTitleData)
        {
            var immutableBuilder = memoryDatabase.ToImmutableBuilder();
            immutableBuilder.ReplaceAll(playFabTitleData.Deserialize<AchievementMaster>(PlayFabConfig.ACHIEVEMENT_KEY));

            this.memoryDatabase = immutableBuilder.Build();
        }
    }
}