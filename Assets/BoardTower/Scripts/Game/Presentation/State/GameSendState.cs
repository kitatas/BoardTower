using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameSendState : BaseGameState
    {
        private readonly LoadingUseCase _loadingUseCase;
        private readonly RoundClearUseCase _roundClearUseCase;
        private readonly SendUseCase _sendUseCase;

        public GameSendState(LoadingUseCase loadingUseCase, RoundClearUseCase roundClearUseCase,
            SendUseCase sendUseCase)
        {
            _loadingUseCase = loadingUseCase;
            _roundClearUseCase = roundClearUseCase;
            _sendUseCase = sendUseCase;
        }

        public override GameState state => GameState.Send;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _loadingUseCase.FadeAsync(Fade.In, token);
            await (
                _sendUseCase.SendScoreAsync(token),
                _sendUseCase.UpdateProgressAsync(_roundClearUseCase.IsClear(), token)
            );
            await _loadingUseCase.FadeAsync(Fade.Out, token);

            return GameState.Finish;
        }
    }
}