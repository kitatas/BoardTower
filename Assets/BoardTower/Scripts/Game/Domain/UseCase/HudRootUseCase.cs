using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class HudRootUseCase
    {
        private readonly HudRootPorts _hudRootPorts;

        public HudRootUseCase(HudRootPorts hudRootPorts)
        {
            _hudRootPorts = hudRootPorts;
        }

        public IAsyncSubscriber<HudRootTransitionVO> transition => _hudRootPorts.hudRootTransitionSubscriber;

        public UniTask InitAsync(CancellationToken token)
        {
            return FadeAsync(Fade.Out, 0.0f, token);
        }

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            return FadeAsync(fade, HudRootConfig.FADE_DURATION, token);
        }

        private UniTask FadeAsync(Fade fade, float duration, CancellationToken token)
        {
            var hudRootTransition = HudRootTransitionVO.Create(fade, duration);
            return _hudRootPorts.PublishHudRootTransitionAsync(hudRootTransition, token);
        }
    }
}