using BoardTower.Game.Application;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameInitState : BaseGameState
    {
        public override GameState state => GameState.Init;
    }
}