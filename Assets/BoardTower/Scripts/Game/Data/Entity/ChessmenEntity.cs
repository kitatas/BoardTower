using BoardTower.Game.Application;

namespace BoardTower.Game.Data.Entity
{
    public sealed class ChessmenEntity
    {
        public ChessmenType chessmenType { get; private set; }

        public void Set(ChessmenType type)
        {
            chessmenType = type;
        }
    }
}