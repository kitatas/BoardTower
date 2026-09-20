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
        private readonly AppVersionUseCase _appVersionUseCase;
        private readonly GameModeUseCase _gameModeUseCase;
        private readonly DisplayNameUseCase _displayNameUseCase;
        private readonly LoadingUseCase _loadingUseCase;
        private readonly LoginUseCase _loginUseCase;
        private readonly UpdateUseCase _updateUseCase;

        public BootLoginState(AppVersionUseCase appVersionUseCase, GameModeUseCase gameModeUseCase,
            DisplayNameUseCase displayNameUseCase, LoadingUseCase loadingUseCase, LoginUseCase loginUseCase,
            UpdateUseCase updateUseCase)
        {
            _appVersionUseCase = appVersionUseCase;
            _gameModeUseCase = gameModeUseCase;
            _displayNameUseCase = displayNameUseCase;
            _loadingUseCase = loadingUseCase;
            _loginUseCase = loginUseCase;
            _updateUseCase = updateUseCase;
        }

        public override BootState state => BootState.Login;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await (
                _gameModeUseCase.InitAsync(token),
                _displayNameUseCase.InitAsync(token),
                _updateUseCase.InitAsync(token)
            );
        }

        public override async UniTask<BootState> TickAsync(CancellationToken token)
        {
            await _loadingUseCase.FadeAsync(Fade.In, token);

            await _gameModeUseCase.JudgeGameMode(token);

            return await (
                _gameModeUseCase.isOnlineMode
                    ? LoginAsync(token)
                    : LoginOfflineAsync(token)
            );
        }

        private async UniTask<BootState> LoginAsync(CancellationToken token)
        {
            var loginResult = await _loginUseCase.LoginAsync(token);
            if (!loginResult.isSuccess) throw new RetryExceptionVO(ExceptionConfig.FAILED_TO_LOGIN);

            if (_appVersionUseCase.IsForceUpdate())
            {
                await _loadingUseCase.FadeAsync(Fade.Out, token);
                await _updateUseCase.FadeAsync(Fade.In, token);
                return BootState.None;
            }

            if (!loginResult.isRegistered)
            {
                await _loadingUseCase.FadeAsync(Fade.Out, token);
                var userDisplayName = await _displayNameUseCase.DecideDisplayNameAsync(token);

                await _loadingUseCase.FadeAsync(Fade.In, token);
                await _loginUseCase.RegisterAsync(userDisplayName, token);
            }

            return BootState.Load;
        }

        private async UniTask<BootState> LoginOfflineAsync(CancellationToken token)
        {
            await _loadingUseCase.FadeAsync(Fade.Out, token);

            await _loginUseCase.InitDummyAsync(token);

            // Offline起動の通知
            await _gameModeUseCase.FadeInAsync(token);

            return BootState.Load;
        }
    }
}