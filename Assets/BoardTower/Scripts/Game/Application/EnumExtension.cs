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

        public static SquareEventType ToSquareEventType(this int self)
        {
            return FastEnum.IsDefined<SquareEventType>(self)
                ? (SquareEventType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT);
        }

        public static bool IsBeltEvent(this SquareEventType self)
        {
            return self is SquareEventType.BeltUp or
                SquareEventType.BeltDown or
                SquareEventType.BeltLeft or
                SquareEventType.BeltRight;
        }

        public static ChessmenMovementOffsetVO ToBeltOffset(this SquareEventType self)
        {
            return self switch
            {
                SquareEventType.BeltUp => new ChessmenMovementOffsetVO(0, 1),
                SquareEventType.BeltDown => new ChessmenMovementOffsetVO(0, -1),
                SquareEventType.BeltLeft => new ChessmenMovementOffsetVO(-1, 0),
                SquareEventType.BeltRight => new ChessmenMovementOffsetVO(1, 0),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT),
            };
        }
    }
}