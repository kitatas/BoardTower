using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Common.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Presentation.State
{
    public sealed class BootInitState : BaseBootState
    {
        private readonly BgmUseCase _bgmUseCase;
        private readonly SeUseCase _seUseCase;
        private readonly ExceptionUseCase _exceptionUseCase;
        private readonly LoadingUseCase _loadingUseCase;
        private readonly ResourceUseCase _resourceUseCase;

        public BootInitState(BgmUseCase bgmUseCase, SeUseCase seUseCase, ExceptionUseCase exceptionUseCase,
            LoadingUseCase loadingUseCase, ResourceUseCase resourceUseCase)
        {
            _bgmUseCase = bgmUseCase;
            _seUseCase = seUseCase;
            _exceptionUseCase = exceptionUseCase;
            _loadingUseCase = loadingUseCase;
            _resourceUseCase = resourceUseCase;
        }

        public override BootState state => BootState.Init;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            // SaveData読込のロック回避で順次実行
            await _bgmUseCase.LoadAsync(token);
            await _seUseCase.LoadAsync(token);
            await (
                _exceptionUseCase.FadeOutAsync(0.0f, token),
                _loadingUseCase.InitAsync(token),
                _resourceUseCase.LoadAsync(token)
            );
        }

        public override UniTask<BootState> TickAsync(CancellationToken token)
        {
            return UniTask.FromResult(BootState.Splash);
        }
    }
}