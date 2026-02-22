using BoardTower.Common.Application;
using FastEnumUtility;

namespace BoardTower.Game.Application
{
    public static class EnumExtension
    {
        public static ChessmenType ToChessmenType(this int self)
        {
            return FastEnum.IsDefined<ChessmenType>(self)
                ? (ChessmenType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_CHESSMEN);
        }

        public static ChessmenMovementType ToChessmenMovementType(this int self)
        {
            return FastEnum.IsDefined<ChessmenMovementType>(self)
                ? (ChessmenMovementType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_CHESSMEN_MOVEMENT);
        }
    }
}