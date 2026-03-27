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

        public UniTask InitAsync(FinishType type, CancellationToken token)
        {
            var f = FinishVO.Create(type, Fade.Out, 0.0f);
            return _finishPorts.PublishFinishAsync(f, token);
        }

        public UniTask FadeAsync(FinishType type, Fade fade, CancellationToken token)
        {
            var f = FinishVO.Create(type, fade, UiConfig.DURATION);
            return _finishPorts.PublishFinishAsync(f, token);
        }
    }
}