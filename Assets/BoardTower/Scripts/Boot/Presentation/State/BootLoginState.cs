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
        private readonly DisplayNameUseCase _displayNameUseCase;
        private readonly LoadingUseCase _loadingUseCase;
        private readonly LoginUseCase _loginUseCase;

        public BootLoginState(DisplayNameUseCase displayNameUseCase, LoadingUseCase loadingUseCase,
            LoginUseCase loginUseCase)
        {
            _displayNameUseCase = displayNameUseCase;
            _loadingUseCase = loadingUseCase;
            _loginUseCase = loginUseCase;
        }

        public override BootState state => BootState.Login;

        public override UniTask InitAsync(CancellationToken token)
        {
            return _displayNameUseCase.InitAsync(token);
        }

        public override async UniTask<BootState> TickAsync(CancellationToken token)
        {
            await _loadingUseCase.FadeAsync(Fade.In, token);

            var loginResult = await _loginUseCase.LoginAsync(token);
            if (!loginResult.isSuccess) throw new RetryExceptionVO(ExceptionConfig.FAILED_TO_LOGIN);
            if (!loginResult.isRegistered)
            {
                await _loadingUseCase.FadeAsync(Fade.Out, token);
                var userDisplayName = await _displayNameUseCase.DecideDisplayNameAsync(token);

                await _loadingUseCase.FadeAsync(Fade.In, token);
                await _loginUseCase.RegisterAsync(userDisplayName, token);
            }

            await _loadingUseCase.FadeAsync(Fade.Out, token);

            return BootState.Load;
        }
    }
}