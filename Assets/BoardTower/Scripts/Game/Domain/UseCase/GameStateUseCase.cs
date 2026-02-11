using System.Threading;
using BoardTower.Base.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GameStateUseCase : BaseStateUseCase<GameState>
    {
        public GameStateUseCase(GameStateEntity stateEntity, IAsyncSubscriber<GameState> subscriber,
            IAsyncPublisher<GameState> publisher) : base(stateEntity, subscriber, publisher)
        {
        }

        public override async UniTask InitAsync(CancellationToken token)
        {
            await PublishAsync(GameState.Init, token);
        }
    }
}