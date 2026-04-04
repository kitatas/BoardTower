using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Presentation.State
{
    public sealed class BootLoadState : BaseBootState
    {
        private readonly SceneUseCase _sceneUseCase;

        public BootLoadState(SceneUseCase sceneUseCase)
        {
            _sceneUseCase = sceneUseCase;
        }

        public override BootState state => BootState.Load;

        public override UniTask<BootState> TickAsync(CancellationToken token)
        {
            _sceneUseCase.Load(SceneName.Game, LoadType.Fade);
            return UniTask.FromResult(BootState.None);
        }
    }
}