using BoardTower.Boot.Application;
using BoardTower.Boot.Data.Entity;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class BootModalUseCase : BaseModalUseCase<BootModalType>
    {
        public BootModalUseCase(BootModalEntity entity, BootModalPorts ports) : base(entity, ports)
        {
        }

        protected override BaseModalTransitionVO<BootModalType> GetModalTransition(BaseModalVO<BootModalType> modal)
        {
            return BootModalTransitionVO.Create(modal as BootModalVO, BootModalConfig.FADE_DURATION);
        }
    }
}