using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.DataStore.Tables;
using FastEnumUtility;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class ScoreRateRepository
    {
        private readonly ScoreRateMasterTable _scoreRateMasterTable;

        public ScoreRateRepository(MemoryDatabase memoryDatabase)
        {
            _scoreRateMasterTable = memoryDatabase.ScoreRateMasterTable;
        }

        public ScoreRateVO FindGemRate(int round)
        {
            return Find(ScoreRateType.Gem, round);
        }

        private ScoreRateVO Find(ScoreRateType type, int threshold)
        {
            if (_scoreRateMasterTable.TryFindByTypeAndThreshold((type.ToInt32(), threshold), out var master))
            {
                return master.ToVO();
            }
            else
            {
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SCORE_RATE);
            }
        }
    }
}