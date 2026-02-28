using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameInputState : BaseGameState
    {
        private readonly MovementUseCase _movementUseCase;

        public GameInputState(MovementUseCase movementUseCase)
        {
            _movementUseCase = movementUseCase;
        }

        public override GameState state => GameState.Input;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            await _movementUseCase.PublishMovableSquaresAsync(token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _movementUseCase.MoveAsync(token);
            return GameState.Event;
        }
    }
}