using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Presentation.State
{
    public sealed class BootLoginState : BaseBootState
    {
        private readonly GameModeUseCase _gameModeUseCase;
        private readonly DisplayNameUseCase _displayNameUseCase;
        private readonly LoadingUseCase _loadingUseCase;
        private readonly LoginUseCase _loginUseCase;

        public BootLoginState(GameModeUseCase gameModeUseCase, DisplayNameUseCase displayNameUseCase,
            LoadingUseCase loadingUseCase, LoginUseCase loginUseCase)
        {
            _gameModeUseCase = gameModeUseCase;
            _displayNameUseCase = displayNameUseCase;
            _loadingUseCase = loadingUseCase;
            _loginUseCase = loginUseCase;
        }

        public override BootState state => BootState.Login;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await (
                _gameModeUseCase.InitAsync(token),
                _displayNameUseCase.InitAsync(token)
            );
        }

        public override async UniTask<BootState> TickAsync(CancellationToken token)
        {
            await _loadingUseCase.FadeAsync(Fade.In, token);

            await _gameModeUseCase.JudgeGameMode(token);
            if (_gameModeUseCase.isOnlineMode)
            {
                await LoginAsync(token);
            }
            else
            {
                await _loginUseCase.InitDummyAsync(token);

                // Offline起動の通知
                await _gameModeUseCase.FadeInAsync(token);
            }

            return BootState.Load;
        }

        private async UniTask LoginAsync(CancellationToken token)
        {
            var loginResult = await _loginUseCase.LoginAsync(token);
            if (!loginResult.isSuccess) throw new RetryExceptionVO(ExceptionConfig.FAILED_TO_LOGIN);
            if (!loginResult.isRegistered)
            {
                await _loadingUseCase.FadeAsync(Fade.Out, token);
                var userDisplayName = await _displayNameUseCase.DecideDisplayNameAsync(token);

                await _loadingUseCase.FadeAsync(Fade.In, token);
                await _loginUseCase.RegisterAsync(userDisplayName, token);
            }
        }
    }
}