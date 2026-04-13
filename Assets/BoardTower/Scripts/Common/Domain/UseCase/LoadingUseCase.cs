using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class LoadingUseCase
    {
        private readonly LoadingPorts _loadingPorts;

        public LoadingUseCase(LoadingPorts loadingPorts)
        {
            _loadingPorts = loadingPorts;
        }

        public IAsyncSubscriber<LoadingTransitionVO> transition => _loadingPorts.loadingTransitionSubscriber;

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var loadingTransition = LoadingTransitionVO.Create(fade, LoadingConfig.FADE_DURATION);
            return _loadingPorts.PublishLoadingTransitionAsync(loadingTransition, token);
        }
    }
}