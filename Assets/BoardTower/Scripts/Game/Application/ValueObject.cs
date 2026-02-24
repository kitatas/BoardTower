using System.Linq;
using BoardTower.Common.Application;

namespace BoardTower.Game.Application
{
    public sealed class BoardTransitionVO : TransitionVO
    {
        public BoardTransitionVO(Fade fade, float duration = 0) : base(fade, duration)
        {
        }
    }

    public sealed class ChessmenTransitionVO : TransitionVO
    {
        public ChessmenTransitionVO(Fade fade, float duration = 0) : base(fade, duration)
        {
        }
    }

    public sealed class ChessmenMovementRuleVO
    {
        public readonly ChessmenType type;
        public readonly ChessmenMovementType movement;
        public readonly ChessmenMovementOffsetVO[] offsets;

        public ChessmenMovementRuleVO(int type, int movement, (int dx, int dy)[] offsets)
        {
            var chessmenType = type.ToChessmenType();
            if (chessmenType is ChessmenType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_CHESSMEN);

            var movementType = movement.ToChessmenMovementType();
            if (movementType is ChessmenMovementType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_CHESSMEN_MOVEMENT);

            this.type = chessmenType;
            this.movement = movementType;
            this.offsets = offsets
                .Select(x => new ChessmenMovementOffsetVO(x.dx, x.dy))
                .ToArray();
        }
    }

    public sealed class ChessmenMovementOffsetVO
    {
        public readonly int dx;
        public readonly int dy;

        public ChessmenMovementOffsetVO(int dx, int dy)
        {
            this.dx = dx;
            this.dy = dy;
        }
    }

    public sealed class HighlightSquareVO
    {
        public readonly int file;
        public readonly int rank;
        public readonly HighlightSquareType highlight;

        public HighlightSquareVO(int file, int rank, HighlightSquareType highlight)
        {
            if (highlight is HighlightSquareType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_HIGHLIGHT);

            this.file = file;
            this.rank = rank;
            this.highlight = highlight;
        }
    }

    public sealed class ClickSquareVO
    {
        public readonly int file;
        public readonly int rank;

        public ClickSquareVO(int file, int rank)
        {
            this.file = file;
            this.rank = rank;
        }

        public override string ToString() => $"file: {file}, rank: {rank}";
    }
}