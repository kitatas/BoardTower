using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameJudgeState : BaseGameState
    {
        public override GameState state => GameState.Judge;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            // TODO: round目標達成しているか
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                return GameState.SetUp;
            }
        }
    }
}