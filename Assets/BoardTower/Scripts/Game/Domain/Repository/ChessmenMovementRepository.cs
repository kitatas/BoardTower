using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.DataStore.Tables;
using FastEnumUtility;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class ChessmenMovementRepository
    {
        private readonly ChessmenMovementRuleMasterTable _chessmenMovementRuleMasterTable;

        public ChessmenMovementRepository(MemoryDatabase memoryDatabase)
        {
            _chessmenMovementRuleMasterTable = memoryDatabase.ChessmenMovementRuleMasterTable;
        }

        public ChessmenMovementRuleVO Find(ChessmenType type)
        {
            if (_chessmenMovementRuleMasterTable.TryFindByType(type.ToInt32(), out var master))
            {
                return master.ToVO();
            }
            else
            {
                throw new QuitExceptionVO(ExceptionConfig.INVALID_CHESSMEN_MOVEMENT);
            }
        }
    }
}