using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
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

        public UniTask InitAsync(CancellationToken token)
        {
            var u = UpdateTransitionVO.Create(Fade.Out, 0.0f);
            return _updatePorts.PublishUpdateTransitionAsync(u, token);
        }

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var u = UpdateTransitionVO.Create(fade, UpdateConfig.FADE_DURATION);
            return _updatePorts.PublishUpdateTransitionAsync(u, token);
        }
    }
}