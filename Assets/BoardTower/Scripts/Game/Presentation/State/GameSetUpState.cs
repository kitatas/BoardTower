using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameSetUpState : BaseGameState
    {
        private readonly BoardUseCase _boardUseCase;

        public GameSetUpState(BoardUseCase boardUseCase)
        {
            _boardUseCase = boardUseCase;
        }

        public override GameState state => GameState.SetUp;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await _boardUseCase.InitAsync(token);
        }

        public override async UniTask EnterAsync(CancellationToken token)
        {
            await _boardUseCase.FadeAsync(Fade.In, token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            // TODO: next state
            return GameState.None;
        }
    }
}