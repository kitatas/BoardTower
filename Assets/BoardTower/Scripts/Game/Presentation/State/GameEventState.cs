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
        private readonly GemComboUseCase _gemComboUseCase;
        private readonly PlyUseCase _plyUseCase;
        private readonly ScoreUseCase _scoreUseCase;

        public GameEventState(EventUseCase eventUseCase, GemUseCase gemUseCase, GemComboUseCase gemComboUseCase,
            PlyUseCase plyUseCase, ScoreUseCase scoreUseCase)
        {
            _eventUseCase = eventUseCase;
            _gemUseCase = gemUseCase;
            _gemComboUseCase = gemComboUseCase;
            _plyUseCase = plyUseCase;
            _scoreUseCase = scoreUseCase;
        }

        public override GameState state => GameState.Event;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            var result = await _eventUseCase.ApplyEventAsync(token);

            // Belt 系であれば、移動後の SquareEvent 実行
            if (result.isBelt) return state;

            _gemComboUseCase.Apply(result.type);

            if (result.gemNum > 0)
            {
                _gemUseCase.Add(result.gemNum);
                _scoreUseCase.ApplyGemScore(result.gemNum);
            }

            if (result.plyNum > 0) _plyUseCase.Add(result.plyNum);

            return _plyUseCase.IsZero() ? GameState.Judge : GameState.Input;
        }
    }
}