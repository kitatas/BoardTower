using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Common.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Presentation.State
{
    public sealed class BootInitState : BaseBootState
    {
        private readonly ExceptionUseCase _exceptionUseCase;

        public BootInitState(ExceptionUseCase exceptionUseCase)
        {
            _exceptionUseCase = exceptionUseCase;
        }

        public override BootState state => BootState.Init;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            await (
                _exceptionUseCase.FadeOutAsync(0.0f, token)
            );
        }

        public override UniTask<BootState> TickAsync(CancellationToken token)
        {
            return UniTask.FromResult(BootState.Splash);
        }
    }
}