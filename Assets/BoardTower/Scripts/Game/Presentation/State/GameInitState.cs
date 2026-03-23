using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameInitState : BaseGameState
    {
        private readonly ChessmenUseCase _chessmenUseCase;

        public GameInitState(ChessmenUseCase chessmenUseCase)
        {
            _chessmenUseCase = chessmenUseCase;
        }

        public override GameState state => GameState.Init;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            _chessmenUseCase.Init();
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            return GameState.SetUp;
        }
    }
}