using BoardTower.Base.Presentation.State;
using BoardTower.Game.Application;

namespace BoardTower.Game.Presentation.State
{
    public abstract class BaseGameState : BaseState<GameState>
    {
        public override GameState state { get; }
    }
}