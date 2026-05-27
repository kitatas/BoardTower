using System;
using System.Threading;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class DisplayNameUseCase : IDisposable
    {
        private readonly DisplayNamePorts _displayNamePorts;
        private readonly Subject<UserDisplayNameVO> _displayName;

        public DisplayNameUseCase(DisplayNamePorts displayNamePorts)
        {
            _displayNamePorts = displayNamePorts;
            _displayName = new Subject<UserDisplayNameVO>();
        }

        public IAsyncSubscriber<DisplayNameTransitionVO> transition =>
            _displayNamePorts.displayNameTransitionSubscriber;

        public UniTask InitAsync(CancellationToken token)
        {
            var displayNameTransition = DisplayNameTransitionVO.Create(Fade.Out, 0.0f);
            return _displayNamePorts.PublishDisplayNameTransitionAsync(displayNameTransition, token);
        }

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var displayNameTransition = DisplayNameTransitionVO.Create(fade, UiConfig.DURATION);
            return _displayNamePorts.PublishDisplayNameTransitionAsync(displayNameTransition, token);
        }

        public void HandleDisplayName(UserDisplayNameVO userDisplayName)
        {
            _displayName?.OnNext(userDisplayName);
        }

        public async UniTask<UserDisplayNameVO> DecideDisplayNameAsync(CancellationToken token)
        {
            for (int i = 0; i < PlayFabConfig.DECISION_NAME_RETRY_COUNT; i++)
            {
                await FadeAsync(Fade.In, token);

                var userDisplayName = await _displayName.FirstAsync(token);

                await FadeAsync(Fade.Out, token);

                if (!string.IsNullOrEmpty(userDisplayName.value)) return userDisplayName;
            }

            throw new RebootExceptionVO(ExceptionConfig.FAILED_TO_DECIDE_NAME);
        }

        void IDisposable.Dispose()
        {
            _displayName?.Dispose();
        }
    }
}