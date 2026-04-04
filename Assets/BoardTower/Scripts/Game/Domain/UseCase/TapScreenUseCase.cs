using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class TapScreenUseCase
    {
        private readonly TapScreenPorts _tapScreenPorts;

        public TapScreenUseCase(TapScreenPorts tapScreenPorts)
        {
            _tapScreenPorts = tapScreenPorts;
        }

        public IAsyncSubscriber<TapScreenVO> tapScreen => _tapScreenPorts.tapScreenSubscriber;
    }
}