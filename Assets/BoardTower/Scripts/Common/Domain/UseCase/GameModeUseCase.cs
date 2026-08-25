using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class GameModeUseCase : IDisposable
    {
        private readonly GameModeEntity _gameModeEntity;
        private readonly Subject<GameMode> _gameMode;

        public GameModeUseCase(GameModeEntity gameModeEntity)
        {
            _gameModeEntity = gameModeEntity;
            _gameMode = new Subject<GameMode>();
        }

        public Observable<GameMode> gameMode => _gameMode;
        public bool isOnlineMode => _gameModeEntity.value.mode == GameMode.Online;

        public async UniTask JudgeGameMode(CancellationToken token)
        {
            // UnityEngine.Application.internetReachability はメインスレッドでのみ参照可能なため
            // UniTask.SwitchToMainThread で保証する
            await UniTask.SwitchToMainThread(token);

            var isOnline = UnityEngine.Application.internetReachability != UnityEngine.NetworkReachability.NotReachable;
            var mode = isOnline ? GameMode.Online : GameMode.Offline;
            _gameModeEntity.Set(new GameModeVO(mode));
        }

        public void SetUp()
        {
            _gameMode?.OnNext(_gameModeEntity.value.mode);
        }

        void IDisposable.Dispose()
        {
            _gameMode?.Dispose();
        }
    }
}