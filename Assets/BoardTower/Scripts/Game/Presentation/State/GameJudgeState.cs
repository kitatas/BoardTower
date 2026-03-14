using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameJudgeState : BaseGameState
    {
        private readonly BoardUseCase _boardUseCase;
        private readonly ChessmenUseCase _chessmenUseCase;

        public GameJudgeState(BoardUseCase boardUseCase, ChessmenUseCase chessmenUseCase)
        {
            _boardUseCase = boardUseCase;
            _chessmenUseCase = chessmenUseCase;
        }

        public override GameState state => GameState.Judge;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            // TODO: round目標達成しているか
            {
                await (
                    _boardUseCase.FadeAsync(Fade.Out, token),
                    _chessmenUseCase.FadeAsync(Fade.Out, token)
                );
                return GameState.SetUp;
            }
        }
    }
}