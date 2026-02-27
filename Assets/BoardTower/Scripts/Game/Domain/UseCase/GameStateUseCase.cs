using System.Threading;
using BoardTower.Base.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GameStateUseCase : BaseStateUseCase<GameState>
    {
        public GameStateUseCase(GameStateEntity entity, GameStatePorts ports) : base(entity, ports)
        {
        }

        public override async UniTask InitAsync(CancellationToken token)
        {
            await PublishAsync(GameState.Init, token);
        }

        public override async UniTask PublishAsync(GameState value, CancellationToken token)
        {
            if (value is GameState.None) return;
            await base.PublishAsync(value, token);
        }
    }
}