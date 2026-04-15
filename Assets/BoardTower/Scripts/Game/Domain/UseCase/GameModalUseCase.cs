using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GameModalUseCase : BaseModalUseCase<GameModalType>
    {
        public GameModalUseCase(GameModalEntity entity, GameModalPorts ports) : base(entity, ports)
        {
        }

        public UniTask FadeAsync(GameModalType type, Fade fade, CancellationToken token)
        {
            var modalTransition = GameModalTransitionVO.Create(type, fade, GameModalConfig.FADE_DURATION);
            return PublishAsync(modalTransition, token);
        }
    }
}