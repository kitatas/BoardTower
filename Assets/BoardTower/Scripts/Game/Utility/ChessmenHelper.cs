using System.Collections.Generic;
using BoardTower.Game.Application;

namespace BoardTower.Game.Utility
{
    public static class ChessmenHelper
    {
        public static List<SquareVO> GetMovableSquares(SquareVO origin, ChessmenMovementRuleVO rule)
        {
            var squares = new List<SquareVO>(16);
            foreach (var offset in rule.offsets)
            {
                for (int step = 1;; step++)
                {
                    var file = origin.file + offset.dx * step;
                    var rank = origin.rank + offset.dy * step;
                    if (BoardHelper.IsOutOfBoard(file, rank)) break;

                    squares.Add(new SquareVO(file, rank));

                    // Slider 以外は1手のみ
                    if (rule.movement is not ChessmenMovementType.Slider) break;
                }
            }

            return squares;
        }
        
        public static SquareVO CalcSquare(SquareVO square, ChessmenMovementOffsetVO offset)
        {
            return new SquareVO(square.file + offset.dx, square.rank + offset.dy);
        }
    }
}