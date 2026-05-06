using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Application
{
    public sealed class BoardTransitionVO
    {
        public readonly TransitionVO transition;

        public BoardTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static BoardTransitionVO Create(Fade fade)
        {
            var transition = new TransitionVO(fade, BoardConfig.FADE_DURATION);
            return new BoardTransitionVO(transition);
        }
    }

    public sealed class ChessmenTransitionVO
    {
        public readonly TransitionVO transition;
        public readonly SquareVO square;

        public ChessmenTransitionVO(TransitionVO transition, SquareVO square)
        {
            this.transition = transition;
            this.square = square;
        }

        public static ChessmenTransitionVO Create(Fade fade, SquareVO square)
        {
            var transition = new TransitionVO(fade, ChessmenConfig.FADE_DURATION);
            return new ChessmenTransitionVO(transition, square);
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

    public sealed class HighlightIndexVO
    {
        public readonly int index;
        public readonly HighlightSquareType highlight;

        public HighlightIndexVO(int index, HighlightSquareType highlight)
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

        public static EventSquareVO Create(int file, int rank, SquareEventVO squareEvent)
        {
            var square = new SquareVO(file, rank);
            return new EventSquareVO(square, squareEvent);
        }
    }

    public sealed class RenderEventSquareVO
    {
        public readonly RenderType render;
        public readonly EventSquareVO[] eventSquares;

        public RenderEventSquareVO(RenderType render, EventSquareVO[] eventSquares)
        {
            if (render is RenderType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_RENDER);

            this.render = render;
            this.eventSquares = eventSquares;
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

        public static EventResultVO Create(SquareEventType type)
        {
            return new EventResultVO(
                type.IsBeltEvent() || type is SquareEventType.Block,
                type == SquareEventType.Gem ? 1 : 0,
                type == SquareEventType.Ply ? 1 : 0
            );
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

    public sealed class FinishTransitionVO
    {
        public readonly FinishType type;
        public readonly TransitionVO transition;

        public FinishTransitionVO(FinishType type, TransitionVO transition)
        {
            if (type is FinishType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_FINISH);

            this.type = type;
            this.transition = transition;
        }

        public static FinishTransitionVO Create(FinishType type, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new FinishTransitionVO(type, transition);
        }
    }

    public sealed class TapScreenTransitionVO
    {
        public readonly TransitionVO transition;

        public TapScreenTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static TapScreenTransitionVO Create(Fade fade)
        {
            var transition = new TransitionVO(fade, TapScreenConfig.FADE_DURATION);
            return new TapScreenTransitionVO(transition);
        }
    }

    public sealed class HudRootTransitionVO
    {
        public readonly TransitionVO transition;

        public HudRootTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static HudRootTransitionVO Create(Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new HudRootTransitionVO(transition);
        }
    }

    public sealed class GameModalVO : BaseModalVO<GameModalType>
    {
        public GameModalVO(GameModalType type, Fade fade) : base(type, fade)
        {
            if (type is GameModalType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_GAME_MODAL);
        }
    }

    public sealed class GameModalTransitionVO : BaseModalTransitionVO<GameModalType>
    {
        public GameModalTransitionVO(GameModalType type, TransitionVO transition) : base(type, transition)
        {
            if (type is GameModalType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_GAME_MODAL);
        }

        public static GameModalTransitionVO Create(GameModalType type, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new GameModalTransitionVO(type, transition);
        }

        public static GameModalTransitionVO Create(GameModalVO gameModal, float duration)
        {
            var transition = new TransitionVO(gameModal.fade, duration);
            return new GameModalTransitionVO(gameModal.type, transition);
        }
    }

    public sealed class RelicVO
    {
        public readonly RelicType type;
        public readonly string content;

        public RelicVO(RelicType type, string content)
        {
            if (type is RelicType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_RELIC);

            this.type = type;
            this.content = content;
        }
    }

    public sealed class LotRelicVO
    {
        public readonly IEnumerable<RelicVO> relics;

        public LotRelicVO(IEnumerable<RelicVO> relics)
        {
            this.relics = relics;
        }
    }

    public sealed class LotRelicTransitionVO
    {
        public readonly TransitionVO transition;

        public LotRelicTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static LotRelicTransitionVO Create(Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new LotRelicTransitionVO(transition);
        }
    }

    public sealed class PickRelicVO
    {
        public readonly IEnumerable<RelicVO> relics;

        public PickRelicVO(IEnumerable<RelicVO> relics)
        {
            this.relics = relics;
        }
    }

    public sealed class SelectRelicVO
    {
        public readonly int index;
        public readonly Vector3 position;

        public SelectRelicVO(int index, Vector3 position)
        {
            this.index = index;
            this.position = position;
        }
    }
}