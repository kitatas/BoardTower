namespace BoardTower.Game.Application
{
    public sealed class BoardConfig
    {
        public const int MIN_FILE = 1;
        public const int MAX_FILE = 8;
        public const int HALF_FILE = MAX_FILE / 2;
        public const int MIN_RANK = 1;
        public const int MAX_RANK = 8;
        public const int HALF_RANK = MAX_RANK / 2;
        public const float DELAY_RATE = 0.1f;
        public const float FADE_DURATION = 0.25f;
        public const float HIGHLIGHT_DURATION = 0.5f;
        public const float EVENT_DURATION = 0.0f;
        public const float EVENT_OBJECT_DURATION = 3.0f;

        public static readonly RotateType[] ROTATES = {
            RotateType.Angle0,
            RotateType.Angle90,
            RotateType.Angle180,
            RotateType.Angle270,
        };

        public static readonly SquareEventType[] BELTS =
        {
            SquareEventType.BeltRight,
            SquareEventType.BeltDown,
            SquareEventType.BeltLeft,
            SquareEventType.BeltUp,
        };
    }

    public sealed class ChessmenConfig
    {
        public static readonly ChessmenType DEFAULT_TYPE = ChessmenType.Knight;
        public const float FADE_DURATION = 0.25f;
        public const float MOVE_DURATION = 0.25f;
    }

    public sealed class MovementOffsetConfig
    {
        public static readonly ChessmenMovementOffsetVO UP = new(0, 1);
        public static readonly ChessmenMovementOffsetVO DOWN = new(0, -1);
        public static readonly ChessmenMovementOffsetVO LEFT = new(-1, 0);
        public static readonly ChessmenMovementOffsetVO RIGHT = new(1, 0);
    }

    public sealed class RoundConfig
    {
        public const int MAX_NUM = 7;
    }

    public sealed class TapScreenConfig
    {
        public const float FADE_DURATION = 0.25f;
        public const float FLASH_DURATION = 1.0f;
    }

    public sealed class HudRootConfig
    {
        public const float FADE_DURATION = 0.25f;
        public const float FADE_DELAY_RATE = 0.2f;
    }

    public sealed class GameModalConfig
    {
        public const float FADE_DURATION = 0.25f;
    }

    public sealed class RelicConfig
    {
        public const int LOT_NUM = 3;
        public const float LOT_FADE_DURATION = 0.25f;
        public const float LOT_DELAY_RATE = 0.1f;

        public const int ADDITION_THRESHOLD = 70;
        public const int CONTINUATION_THRESHOLD = 70;
    }

    public sealed class ScoreConfig
    {
        public const int BASE_GEM_VALUE = 100;
        public const int BASE_ROUND_CLEAR_VALUE = 500;
        public const int BASE_RIDE_ON_COLLAPSE_VALUE = 50;

        public const float OVERFLOW_ROUND_GEM_RATE = 2.0f;
    }
}