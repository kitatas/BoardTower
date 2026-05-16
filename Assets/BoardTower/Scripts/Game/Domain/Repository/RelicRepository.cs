using System.Collections.Generic;
using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.DataStore.Tables;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class RelicRepository
    {
        private readonly RelicMasterTable _relicMasterTable;

        public RelicRepository(MemoryDatabase memoryDatabase)
        {
            _relicMasterTable = memoryDatabase.RelicMasterTable;
        }

        public IEnumerable<RelicVO> FindsLotRelics(IEnumerable<RelicType> types)
        {
            var uniqRelics = _relicMasterTable
                .FindByIsUniq(true)
                .Where(x => !types.Contains(x.Type.ToRelicType()));

            var nonUniqRelics = _relicMasterTable
                .FindByIsUniq(false);

            return uniqRelics
                .Union(nonUniqRelics)
                .Select(x => x.ToVO());
        }
    }
}