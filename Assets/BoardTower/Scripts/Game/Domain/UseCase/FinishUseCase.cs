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

        public async UniTask InitAsync(FinishType type, CancellationToken token)
        {
            await FadeAsync(type, Fade.Out, 0.0f, token);
        }

        public async UniTask FadeAsync(FinishType type, Fade fade, CancellationToken token)
        {
            await FadeAsync(type, fade, UiConfig.DURATION, token);
        }

        private async UniTask FadeAsync(FinishType type, Fade fade, float duration, CancellationToken token)
        {
            await _finishPorts.finishPublisher
                .PublishAsync(new FinishVO(type, new TransitionVO(fade, duration)), token);
        }
    }
}