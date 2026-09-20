using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using MessagePipe;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class UpdateUseCase
    {
        private readonly UpdatePorts _updatePorts;

        public UpdateUseCase(UpdatePorts updatePorts)
        {
            _updatePorts = updatePorts;
        }

        public IAsyncSubscriber<UpdateTransitionVO> transition => _updatePorts.updateTransitionSubscriber;
    }
}