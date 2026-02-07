using BoardTower.Base.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GameStateUseCase : BaseStateUseCase<GameState>
    {
        public GameStateUseCase(GameStateEntity stateEntity) : base(stateEntity)
        {
        }

        public override void Init()
        {
            Set(GameState.Init);
        }
    }
}