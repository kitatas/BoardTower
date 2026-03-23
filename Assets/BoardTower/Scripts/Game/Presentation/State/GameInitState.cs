using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameInitState : BaseGameState
    {
        private readonly ChessmenUseCase _chessmenUseCase;
        private readonly RoundUseCase _roundUseCase;

        public GameInitState(ChessmenUseCase chessmenUseCase, RoundUseCase roundUseCase)
        {
            _chessmenUseCase = chessmenUseCase;
            _roundUseCase = roundUseCase;
        }

        public override GameState state => GameState.Init;

        public override async UniTask EnterAsync(CancellationToken token)
        {
            _chessmenUseCase.Init();
            _roundUseCase.Init();
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            return GameState.SetUp;
        }
    }
}