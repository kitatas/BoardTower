using System;
using BoardTower.Common.Application;
using R3;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class SceneUseCase : IDisposable
    {
        private readonly Subject<LoadVO> _load;

        public SceneUseCase()
        {
            _load = new Subject<LoadVO>();
        }

        public Observable<LoadVO> load => _load;

        public void Load(SceneName sceneName, LoadType loadType)
        {
            _load?.OnNext(new LoadVO(sceneName, loadType));
        }

        public void Dispose()
        {
            _load?.Dispose();
        }
    }
}