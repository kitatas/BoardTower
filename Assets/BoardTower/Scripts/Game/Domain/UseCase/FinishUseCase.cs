using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class FinishUseCase
    {
        private readonly FinishPorts _finishPorts;

        public FinishUseCase(FinishPorts finishPorts)
        {
            _finishPorts = finishPorts;
        }

        public IAsyncSubscriber<FinishVO> finish => _finishPorts.finishSubscriber;

        public async UniTask FadeAsync(Fade fade, FinishType type, CancellationToken token)
        {
            await _finishPorts.finishPublisher
                .PublishAsync(new FinishVO(type, new TransitionVO(fade, UiConfig.DURATION)), token);
        }
    }
}