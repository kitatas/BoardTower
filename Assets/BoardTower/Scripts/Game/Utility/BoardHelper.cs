using BoardTower.Common.Application;
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

        public static int ToIndex(int file, int rank)
        {
            return (file - 1) * 8 + (rank - 1);
        }

        public static (int file, int rank) ToFileRank(int index)
        {
            var files = BoardConfig.MAX_FILE - BoardConfig.MIN_FILE + 1;
            var ranks = BoardConfig.MAX_RANK - BoardConfig.MIN_RANK + 1;
            var maxIndex = files * ranks;

            if (!index.IsBetween(0, maxIndex - 1))
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_INDEX);

            var file = (index / ranks) + BoardConfig.MIN_FILE;
            var rank = (index % ranks) + BoardConfig.MIN_RANK;
            return (file, rank);
        }
    }
}