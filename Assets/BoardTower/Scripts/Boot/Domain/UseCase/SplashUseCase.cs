using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.Repository;
using BoardTower.Common.Application;
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

        public UniTask InitAsync(CancellationToken token)
        {
            // NOTE: FadeOut時はSplash不要なのでnull指定
            var splashTransition = SplashTransitionVO.Create(null, Fade.Out, 0.0f);
            return _splashPorts.PublishSplashTransitionAsync(splashTransition, token);
        }

        public async UniTask SequentialRenderAsync(CancellationToken token)
        {
            foreach (var type in SplashConfig.TYPES)
            {
                await FadeAsync(type, token);
            }
        }

        private UniTask FadeAsync(SplashType type, CancellationToken token)
        {
            var splash = _splashRepository.Find(type);
            var splashTransition = SplashTransitionVO.Create(splash, Fade.InOut, SplashConfig.FADE_DURATION);
            return _splashPorts.PublishSplashTransitionAsync(splashTransition, token);
        }
    }
}