using System.Threading;
using BoardTower.Game.Application;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameFinishState : BaseGameState
    {
        public override GameState state => GameState.Finish;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            return GameState.Init;
        }
    }
}