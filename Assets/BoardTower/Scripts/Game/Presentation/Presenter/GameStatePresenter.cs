using System.Collections.Generic;
using BoardTower.Base.Presentation.Presenter;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.State;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class GameStatePresenter : BaseStatePresenter<GameState>
    {
        public GameStatePresenter(GameStateUseCase stateUseCase, IEnumerable<BaseGameState> states) : base(stateUseCase, states)
        {
        }
    }
}