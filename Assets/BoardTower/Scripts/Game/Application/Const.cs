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
        public const float FADE_DURATION = 0.5f;
        public const float HIGHLIGHT_DURATION = 0.5f;
        public const float EVENT_DURATION = 0.0f;

        public static readonly RotateType[] ROTATES = {
            RotateType.Angle0,
            RotateType.Angle90,
            RotateType.Angle180,
            RotateType.Angle270,
        };
    }

    public sealed class ChessmenConfig
    {
        public const float FADE_DURATION = 0.5f;
        public const float MOVE_DURATION = 0.5f;
    }

    public sealed class RoundConfig
    {
        public const int MAX_NUM = 7;
    }
}