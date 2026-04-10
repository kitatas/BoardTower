using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.Repository;
using BoardTower.Common.Application;
using BoardTower.Common.Utility;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class SplashUseCase
    {
        private readonly SplashPorts _splashPorts;
        private readonly SplashRepository _splashRepository;

        public SplashUseCase(SplashPorts splashPorts, SplashRepository splashRepository)
        {
            _splashPorts = splashPorts;
            _splashRepository = splashRepository;
        }

        public IAsyncSubscriber<SplashTransitionVO> transition => _splashPorts.splashTransitionSubscriber;

        public async UniTask SequentialRenderAsync(CancellationToken token)
        {
            foreach (var type in SplashConfig.TYPES)
            {
                await FadeAsync(type, token);
            }
        }

        private async UniTask FadeAsync(SplashType type, CancellationToken token)
        {
            var splash = _splashRepository.Find(type);
            {
                var splashTransition = SplashTransitionVO.Create(splash, Fade.In, SplashConfig.FADE_DURATION);
                await _splashPorts.PublishSplashTransitionAsync(splashTransition, token);
            }
            await UniTaskHelper.DelayAsync(SplashConfig.DISPLAY_DURATION, token);
            {
                var splashTransition = SplashTransitionVO.Create(splash, Fade.Out, SplashConfig.FADE_DURATION);
                await _splashPorts.PublishSplashTransitionAsync(splashTransition, token);
            }
        }
    }
}