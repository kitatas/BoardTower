using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class TapScreenUseCase : IDisposable
    {
        private readonly TapScreenPorts _tapScreenPorts;
        private readonly Subject<Unit> _tapScreen;

        public TapScreenUseCase(TapScreenPorts tapScreenPorts)
        {
            _tapScreenPorts = tapScreenPorts;
            _tapScreen = new Subject<Unit>();
        }

        public IAsyncSubscriber<TapScreenVO> tapScreen => _tapScreenPorts.tapScreenSubscriber;

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var tap = TapScreenVO.Create(fade);
            return _tapScreenPorts.PublishTapScreenAsync(tap, token);
        }

        public void TapScreen()
        {
            _tapScreen?.OnNext(Unit.Default);
        }

        public UniTask TapScreenAsync(CancellationToken token)
        {
            return _tapScreen
                .FirstAsync(cancellationToken: token)
                .AsUniTask();
        }

        void IDisposable.Dispose()
        {
            _tapScreen?.Dispose();
        }
    }
}