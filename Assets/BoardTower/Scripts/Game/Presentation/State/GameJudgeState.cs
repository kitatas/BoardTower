using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameJudgeState : BaseGameState
    {
        private readonly RoundClearUseCase _roundClearUseCase;

        public GameJudgeState(RoundClearUseCase roundClearUseCase)
        {
            _roundClearUseCase = roundClearUseCase;
        }

        public override GameState state => GameState.Judge;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            if (_roundClearUseCase.IsClear())
            {
                return GameState.SetUp;
            }
            else
            {
                // TODO: 失敗
                return GameState.None;
            }
        }
    }
}