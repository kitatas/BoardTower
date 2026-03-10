using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameEventState : BaseGameState
    {
        private readonly EventUseCase _eventUseCase;
        private readonly GemUseCase _gemUseCase;

        public GameEventState(EventUseCase eventUseCase, GemUseCase gemUseCase)
        {
            _eventUseCase = eventUseCase;
            _gemUseCase = gemUseCase;
        }

        public override GameState state => GameState.Event;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            var result = await _eventUseCase.ApplyEventAsync(token);

            // Belt 系であれば、移動後の SquareEvent 実行
            if (result.isBelt) return state;

            // TODO: 獲得数の算出
            if (result.gemNum > 0) _gemUseCase.Add(result.gemNum);

            return GameState.Input;
        }
    }
}