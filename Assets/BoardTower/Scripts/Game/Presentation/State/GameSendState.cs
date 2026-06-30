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
        private readonly SendUseCase _sendUseCase;

        public GameSendState(LoadingUseCase loadingUseCase, SendUseCase sendUseCase)
        {
            _loadingUseCase = loadingUseCase;
            _sendUseCase = sendUseCase;
        }

        public override GameState state => GameState.Send;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _loadingUseCase.FadeAsync(Fade.In, token);
            await _sendUseCase.SendScoreAsync(token);
            await _loadingUseCase.FadeAsync(Fade.Out, token);

            return GameState.Finish;
        }
    }
}