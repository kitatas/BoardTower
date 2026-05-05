using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameJudgeState : BaseGameState
    {
        private readonly RoundUseCase _roundUseCase;
        private readonly RoundClearUseCase _roundClearUseCase;

        public GameJudgeState(RoundUseCase roundUseCase, RoundClearUseCase roundClearUseCase)
        {
            _roundUseCase = roundUseCase;
            _roundClearUseCase = roundClearUseCase;
        }

        public override GameState state => GameState.Judge;

        public override UniTask<GameState> TickAsync(CancellationToken token)
        {
            var nextState = _roundClearUseCase.IsClear()
                ? _roundUseCase.IsMaxRound() ? GameState.Clear : GameState.Pick
                : GameState.Fail;
            return UniTask.FromResult(nextState);
        }
    }
}