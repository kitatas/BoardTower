using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Presentation.State
{
    public sealed class BootSplashState : BaseBootState
    {
        private readonly SplashUseCase _splashUseCase;

        public BootSplashState(SplashUseCase splashUseCase)
        {
            _splashUseCase = splashUseCase;
        }

        public override BootState state => BootState.Splash;

        public override UniTask InitAsync(CancellationToken token)
        {
            return _splashUseCase.InitAsync(token);
        }

        public override async UniTask<BootState> TickAsync(CancellationToken token)
        {
            await _splashUseCase.SequentialRenderAsync(token);

            return BootState.Load;
        }
    }
}