using BoardTower.Game.Application;

namespace BoardTower.Game.Utility
{
    public static class ChessmenHelper
    {
        public static SquareVO CalcSquare(SquareVO square, ChessmenMovementOffsetVO offset)
        {
            return new SquareVO(square.file + offset.dx, square.rank + offset.dy);
        }
    }
}