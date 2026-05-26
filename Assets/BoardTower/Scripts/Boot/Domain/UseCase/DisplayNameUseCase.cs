using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class DisplayNameUseCase
    {
        private readonly DisplayNamePorts _displayNamePorts;

        public DisplayNameUseCase(DisplayNamePorts displayNamePorts)
        {
            _displayNamePorts = displayNamePorts;
        }

        public IAsyncSubscriber<DisplayNameTransitionVO> transition =>
            _displayNamePorts.displayNameTransitionSubscriber;

        public UniTask InitAsync(CancellationToken token)
        {
            var displayNameTransition = DisplayNameTransitionVO.Create(Fade.Out, 0.0f);
            return _displayNamePorts.PublishDisplayNameTransitionAsync(displayNameTransition, token);
        }
    }
}