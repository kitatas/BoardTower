using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameEventState : BaseGameState
    {
        private readonly EventUseCase _eventUseCase;

        public GameEventState(EventUseCase eventUseCase)
        {
            _eventUseCase = eventUseCase;
        }

        public override GameState state => GameState.Event;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            var isRetry = await _eventUseCase.ApplyEventAsync(token);
            return isRetry ? state : GameState.Input;
        }
    }
}