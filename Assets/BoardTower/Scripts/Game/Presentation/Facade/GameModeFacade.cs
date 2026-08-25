using BoardTower.Common.Application;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class GameModeFacade
    {
        public GameModeFacade()
        {
        }

        public void Tween(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.Online:
                    break;
                case GameMode.Offline:
                    // TODO: 専用表示・一部ボタン無効化
                    break;
                default:
                    throw new QuitExceptionVO(ExceptionConfig.INVALID_GAME_MODE);
            }
        }
    }
}