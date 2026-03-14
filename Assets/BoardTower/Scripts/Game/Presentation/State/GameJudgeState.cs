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
        private readonly RoundClearUseCase _roundClearUseCase;

        public GameJudgeState(BoardUseCase boardUseCase, ChessmenUseCase chessmenUseCase,
            RoundClearUseCase roundClearUseCase)
        {
            _boardUseCase = boardUseCase;
            _chessmenUseCase = chessmenUseCase;
            _roundClearUseCase = roundClearUseCase;
        }

        public override GameState state => GameState.Judge;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            if (_roundClearUseCase.IsClear())
            {
                await (
                    _boardUseCase.FadeAsync(Fade.Out, token),
                    _chessmenUseCase.FadeAsync(Fade.Out, token)
                );
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