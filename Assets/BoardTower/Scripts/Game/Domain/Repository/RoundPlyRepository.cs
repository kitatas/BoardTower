using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.DataStore.Tables;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class RoundPlyRepository
    {
        private readonly RoundPlyMasterTable _roundPlyMasterTable;

        public RoundPlyRepository(MemoryDatabase memoryDatabase)
        {
            _roundPlyMasterTable = memoryDatabase.RoundPlyMasterTable;
        }

        public RoundPlyVO Find(int round)
        {
            if (_roundPlyMasterTable.TryFindByRound(round, out var master))
            {
                return master.ToVO();
            }
            else
            {
                throw new QuitExceptionVO(ExceptionConfig.INVALID_ROUND_PLY);
            }
        }
    }
}