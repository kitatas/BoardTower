using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GameModalUseCase : BaseModalUseCase<GameModalType>
    {
        public GameModalUseCase(GameModalEntity entity, GameModalPorts ports) : base(entity, ports)
        {
        }

        protected override BaseModalTransitionVO<GameModalType> GetModalTransition(BaseModalVO<GameModalType> modal)
        {
            return null;
        }
    }
}