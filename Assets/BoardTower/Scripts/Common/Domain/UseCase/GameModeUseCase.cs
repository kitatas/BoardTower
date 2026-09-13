using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class GameModeUseCase
    {
        private readonly GameModeEntity _gameModeEntity;
        private readonly GameModePorts _gameModePorts;

        public GameModeUseCase(GameModeEntity gameModeEntity, GameModePorts gameModePorts)
        {
            _gameModeEntity = gameModeEntity;
            _gameModePorts = gameModePorts;
        }

        public IAsyncSubscriber<GameModeTransitionVO> gameModeTransition => _gameModePorts.gameModeTransitionSubscriber;
        public bool isOnlineMode => _gameModeEntity.value.mode == GameMode.Online;

        public UniTask InitAsync(CancellationToken token)
        {
            // NOTE: 初期化前なので、Offline固定に
            var m = GameModeTransitionVO.Create(GameMode.Offline, Fade.Out, 0.0f);
            return _gameModePorts.PublishGameModeAsync(m, token);
        }

        public async UniTask JudgeGameMode(CancellationToken token)
        {
            // UnityEngine.Application.internetReachability はメインスレッドでのみ参照可能なため
            // UniTask.SwitchToMainThread で保証する
            await UniTask.SwitchToMainThread(token);

            var isOnline = UnityEngine.Application.internetReachability != UnityEngine.NetworkReachability.NotReachable;
            var mode = isOnline ? GameMode.Online : GameMode.Offline;
            _gameModeEntity.Set(new GameModeVO(mode));
        }

        public UniTask FadeInAsync(CancellationToken token)
        {
            var m = GameModeTransitionVO.Create(_gameModeEntity.value.mode, Fade.In, GameModeConfig.FADE_DURATION);
            return _gameModePorts.PublishGameModeAsync(m, token);
        }
    }
}