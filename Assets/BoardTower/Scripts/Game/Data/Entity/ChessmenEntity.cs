using BoardTower.Game.Application;

namespace BoardTower.Game.Data.Entity
{
    public sealed class ChessmenEntity
    {
        public ChessmenType chessmenType { get; private set; }
        public SquareVO square { get; private set; }

        public void Init()
        {
            Set(ChessmenType.Knight);
            Set(new SquareVO(BoardConfig.MIN_FILE, BoardConfig.MIN_RANK));
        }

        public void Set(ChessmenType type)
        {
            chessmenType = type;
        }

        public void Set(SquareVO vo)
        {
            square = vo;
        }
    }
}