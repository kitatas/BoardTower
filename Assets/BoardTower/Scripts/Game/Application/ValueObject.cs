using System.Linq;
using BoardTower.Common.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Application
{
    public sealed class BoardTransitionVO : TransitionVO
    {
        public BoardTransitionVO(Fade fade, float duration = 0) : base(fade, duration)
        {
        }
    }

    public sealed class ChessmenTransitionVO
    {
        public readonly Fade fade;
        public readonly SquareVO square;

        public ChessmenTransitionVO(Fade fade, SquareVO square)
        {
            if (fade is Fade.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE);

            this.fade = fade;
            this.square = square;
        }
    }

    public sealed class ChessmenVO
    {
        public readonly ChessmenType type;
        public readonly SquareVO square;

        public ChessmenVO(ChessmenType type, SquareVO square)
        {
            this.type = type;
            this.square = square;
        }

        public ChessmenVO(ChessmenVO chessmen, SquareVO square)
        {
            this.type = chessmen.type;
            this.square = square;
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
        public readonly GameObject eventObject;

        public SquareEventVO(SquareEventType type, GameObject eventObject)
        {
            if (type is SquareEventType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT);

            this.type = type;
            this.eventObject = eventObject;
        }
    }

    public sealed class BoardPatternVO
    {
        public readonly RotateType rotate;
        public readonly SquareEventType[] types;

        public BoardPatternVO(int[] types)
        {
            if (types.Length != 16)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_PATTERN_LENGTH);

            this.rotate = BoardConfig.ROTATES.GetRandom();

            this.types = new SquareEventType[BoardConfig.HALF_FILE * BoardConfig.HALF_RANK];
            for (int file = 0; file < BoardConfig.HALF_FILE; file++)
            {
                for (int rank = 0; rank < BoardConfig.HALF_RANK; rank++)
                {
                    var (nx, ny) = rotate.RotateFileRank(file, rank);
                    var srcIndex = file * BoardConfig.HALF_FILE + rank;
                    var dstIndex = ny * BoardConfig.HALF_RANK + nx;
                    this.types[dstIndex] = rotate.RotateBelt(types[srcIndex].ToSquareEventType());
                }
            }
        }
    }

    public sealed class EventResultVO
    {
        public readonly bool isBelt;
        public readonly int gemNum;
        public readonly int plyNum;

        public EventResultVO(bool isBelt, int gemNum, int plyNum)
        {
            this.isBelt = isBelt;
            this.gemNum = gemNum;
            this.plyNum = plyNum;
        }
    }

    public sealed class RoundPlyVO
    {
        public readonly int round;
        public readonly int plyCount;

        public RoundPlyVO(int round, int plyCount)
        {
            this.round = round;
            this.plyCount = plyCount;
        }
    }

    public sealed class RoundClearVO
    {
        public readonly int round;
        public readonly int gemCount;

        public RoundClearVO(int round, int gemCount)
        {
            this.round = round;
            this.gemCount = gemCount;
        }
    }

    public sealed class FinishVO
    {
        public readonly Fade fade;
        public readonly FinishType type;

        public FinishVO(Fade fade, FinishType type)
        {
            if (fade is Fade.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE);

            if (type is FinishType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_FINISH);

            this.fade = fade;
            this.type = type;
        }
    }
}