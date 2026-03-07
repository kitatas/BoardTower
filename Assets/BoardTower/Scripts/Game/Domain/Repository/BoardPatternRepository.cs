using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.DataStore.Tables;
using UniEx;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class BoardPatternRepository
    {
        private readonly BoardPatternMasterTable _boardPatternMasterTable;

        public BoardPatternRepository(MemoryDatabase memoryDatabase)
        {
            _boardPatternMasterTable = memoryDatabase.BoardPatternMasterTable;
        }

        public BoardPatternVO[] GetRandomPatterns()
        {
            return Enumerable.Range(1, 4)
                .Select(_ => _boardPatternMasterTable.All.GetRandom().ToVO())
                .ToArray();
        }
    }
}