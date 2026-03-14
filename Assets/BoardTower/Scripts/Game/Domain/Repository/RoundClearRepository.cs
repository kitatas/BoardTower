using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.DataStore.Tables;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class RoundClearRepository
    {
        private readonly RoundClearMasterTable _roundClearMasterTable;

        public RoundClearRepository(MemoryDatabase memoryDatabase)
        {
            _roundClearMasterTable = memoryDatabase.RoundClearMasterTable;
        }

        public RoundClearVO Find(int round)
        {
            if (_roundClearMasterTable.TryFindByRound(round, out var master))
            {
                return master.ToVO();
            }
            else
            {
                throw new QuitExceptionVO(ExceptionConfig.INVALID_ROUND_CLEAR);
            }
        }
    }
}