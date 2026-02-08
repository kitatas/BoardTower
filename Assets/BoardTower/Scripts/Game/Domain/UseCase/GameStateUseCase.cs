using BoardTower.Base.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GameStateUseCase : BaseStateUseCase<GameState>
    {
        public GameStateUseCase(GameStateEntity stateEntity) : base(stateEntity)
        {
        }

        public override Observable<GameState> subject => _subject.Where(x => !x.Equals(GameState.None));

        public override void Init()
        {
            Set(GameState.Init);
        }
    }
}