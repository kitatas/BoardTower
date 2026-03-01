using System.Linq;
using BoardTower.Common.Application;
using UnityEngine;

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

    public sealed class SquareVO
    {
        public readonly int file;
        public readonly int rank;

        public SquareVO(int file, int rank)
        {
            this.file = file;
            this.rank = rank;
        }

        public float localX => file - BoardConfig.MIN_FILE - 3.5f;
        public float localZ => rank - BoardConfig.MIN_RANK - 3.5f;

        public bool IsEqual(SquareVO square) => file == square.file && rank == square.rank;
    }

    public sealed class HighlightSquareVO
    {
        public readonly SquareVO square;
        public readonly HighlightSquareType highlight;

        public HighlightSquareVO(SquareVO square, HighlightSquareType highlight)
        {
            if (highlight is HighlightSquareType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_HIGHLIGHT);

            this.square = square;
            this.highlight = highlight;
        }
    }

    public sealed class HighlightVO
    {
        public readonly int index;
        public readonly HighlightSquareType highlight;

        public HighlightVO(int index, HighlightSquareType highlight)
        {
            if (highlight is HighlightSquareType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_HIGHLIGHT);

            this.index = index;
            this.highlight = highlight;
        }
    }

    public sealed class ClickSquareVO
    {
        public readonly SquareVO square;

        public ClickSquareVO(int file, int rank)
        {
            this.square = new SquareVO(file, rank);
        }
    }

    public sealed class EventSquareVO
    {
        public readonly SquareVO square;
        public readonly SquareEventVO squareEvent;

        public EventSquareVO(SquareVO square, SquareEventVO squareEvent)
        {
            this.square = square;
            this.squareEvent = squareEvent;
        }
    }

    public sealed class EventVO
    {
        public readonly int index;
        public readonly Sprite sprite;

        public EventVO(int index, Sprite sprite)
        {
            this.index = index;
            this.sprite = sprite;
        }
    }

    public sealed class ChessmenMovementVO
    {
        public readonly SquareVO square;

        public ChessmenMovementVO(SquareVO square)
        {
            this.square = square;
        }
    }

    public sealed class SquareEventVO
    {
        public readonly SquareEventType type;
        public readonly Sprite sprite;

        public SquareEventVO(SquareEventType type, Sprite sprite)
        {
            this.type = type;
            this.sprite = sprite;
        }
    }
}