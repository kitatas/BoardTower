using BoardTower.Game.Application;
using UniEx;

namespace BoardTower.Game.Utility
{
    public static class BoardHelper
    {
        public static bool IsOutOfBoard(int file, int rank)
        {
            return !file.IsBetween(BoardConfig.MIN_FILE, BoardConfig.MAX_FILE) ||
                   !rank.IsBetween(BoardConfig.MIN_RANK, BoardConfig.MAX_RANK);
        }
    }
}