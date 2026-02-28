namespace BoardTower.Common.Application
{
    public sealed class ExceptionConfig
    {
        public const string REBOOT_MESSAGE = "Return to title.";
        public const string RETRY_MESSAGE = "Retry.";
        public const string QUIT_MESSAGE = "Exit this app.";

        public const string NOT_FOUND_STATE = "NOT_FOUND_STATE";
        public const string INVALID_CHESSMEN = "INVALID_CHESSMEN";
        public const string INVALID_CHESSMEN_MOVEMENT = "INVALID_CHESSMEN_MOVEMENT";
        public const string INVALID_DURATION = "INVALID_DURATION";
        public const string INVALID_FADE = "INVALID_FADE";
        public const string INVALID_HIGHLIGHT = "INVALID_HIGHLIGHT";
        public const string INVALID_SQUARE_INDEX = "INVALID_SQUARE_INDEX";
    }

    public sealed class UiConfig
    {
        public const float DURATION = 0.5f;
    }
}