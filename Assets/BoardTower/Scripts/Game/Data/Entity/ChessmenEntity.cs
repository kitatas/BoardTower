using BoardTower.Game.Application;

namespace BoardTower.Game.Data.Entity
{
    public sealed class ChessmenEntity
    {
        public ChessmenType chessmenType { get; private set; }
        public int file { get; private set; }
        public int rank { get; private set; }

        public void Init()
        {
            Set(ChessmenType.Knight);
            Set(BoardConfig.MIN_FILE, BoardConfig.MIN_RANK);
        }

        public void Set(ChessmenType type)
        {
            chessmenType = type;
        }

        public void Set(int x, int y)
        {
            file = x;
            rank = y;
        }
    }
}