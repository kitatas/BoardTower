using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Utility;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GameSendState : BaseGameState
    {
        private readonly GameModeUseCase _gameModeUseCase;
        private readonly GameModalUseCase _gameModalUseCase;
        private readonly LoadingUseCase _loadingUseCase;
        private readonly RoundClearUseCase _roundClearUseCase;
        private readonly SendUseCase _sendUseCase;

        public GameSendState(GameModeUseCase gameModeUseCase, GameModalUseCase gameModalUseCase,
            LoadingUseCase loadingUseCase, RoundClearUseCase roundClearUseCase, SendUseCase sendUseCase)
        {
            _gameModeUseCase = gameModeUseCase;
            _gameModalUseCase = gameModalUseCase;
            _loadingUseCase = loadingUseCase;
            _roundClearUseCase = roundClearUseCase;
            _sendUseCase = sendUseCase;
        }

        public override GameState state => GameState.Send;

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            if (_gameModeUseCase.isOnlineMode)
            {
                await SendAsync(token);
            }

            return GameState.Finish;
        }

        private async UniTask SendAsync(CancellationToken token)
        {
            await _loadingUseCase.FadeAsync(Fade.In, token);
            await (
                _sendUseCase.SendScoreAsync(token),
                _sendUseCase.UpdateProgressAsync(_roundClearUseCase.IsClear(), token)
            );

            // 更新後の Ranking が取得されるように暫定的な待機
            await UniTaskHelper.DelayAsync(1.0f, token);
            await _loadingUseCase.FadeAsync(Fade.Out, token);

            var modal = new GameModalVO(GameModalType.Ranking, Fade.In);
            await _gameModalUseCase.FadeAsync(modal, token);
        }
    }
}