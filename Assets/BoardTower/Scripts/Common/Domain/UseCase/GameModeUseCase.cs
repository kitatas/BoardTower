using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using Cysharp.Threading.Tasks;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class GameModeUseCase
    {
        private readonly GameModeEntity _gameModeEntity;

        public GameModeUseCase(GameModeEntity gameModeEntity)
        {
            _gameModeEntity = gameModeEntity;
        }

        public bool isOnlineMode => _gameModeEntity.value.isOnlineMode;

        public async UniTask<GameModeVO> JudgeGameMode(CancellationToken token)
        {
            // UnityEngine.Application.internetReachability はメインスレッドでのみ参照可能なため
            // UniTask.SwitchToMainThread で保証する
            await UniTask.SwitchToMainThread(token);

            var isOnline = UnityEngine.Application.internetReachability != UnityEngine.NetworkReachability.NotReachable;
            _gameModeEntity.Set(new GameModeVO(isOnline));
            return _gameModeEntity.value;
        }
    }
}