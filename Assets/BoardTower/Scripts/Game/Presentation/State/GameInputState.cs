using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameInputState : BaseGameState
    {
        private readonly MovementUseCase _movementUseCase;
        private readonly PlyUseCase _plyUseCase;

        public GameInputState(MovementUseCase movementUseCase, PlyUseCase plyUseCase)
        {
            _movementUseCase = movementUseCase;
            _plyUseCase = plyUseCase;
        }

        public override GameState state => GameState.Input;

        public override UniTask EnterAsync(CancellationToken token)
        {
            return _movementUseCase.PublishMovableSquaresAsync(token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _movementUseCase.InputAsync(token);
            _movementUseCase.ClearHighlightSquareAsync(token).Forget();
            _plyUseCase.Decrease();
            await _movementUseCase.MoveAsync(token);

            return GameState.Event;
        }

        public override async UniTask ForceExitAsync(CancellationToken token)
        {
            await (
                _movementUseCase.ClearHighlightSquareAsync(token)
            );
        }
    }
}