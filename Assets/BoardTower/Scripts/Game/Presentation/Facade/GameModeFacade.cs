using BoardTower.Common.Application;
using BoardTower.Game.Presentation.View;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class GameModeFacade
    {
        private readonly GameModeView _gameModeView;

        public GameModeFacade(GameModeView gameModeView)
        {
            _gameModeView = gameModeView;
        }

        public void Tween(GameMode mode)
        {
            _ = mode switch
            {
                GameMode.Online => _gameModeView.ShowOnline(GameModeConfig.TWEEN_DURATION),
                GameMode.Offline => _gameModeView.ShowOffline(GameModeConfig.TWEEN_DURATION),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_GAME_MODE),
            };
        }
    }
}