using System;
using System.Threading;
using BoardTower.Common.Application;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class DisplayNameUseCase : IDisposable
    {
        private readonly Subject<string> _displayName;
        private bool _isDisposed;

        public DisplayNameUseCase()
        {
            _displayName = new Subject<string>();
            _isDisposed = false;
        }

        public void HandleDisplayName(string name)
        {
            if (_isDisposed) return;
            _displayName?.OnNext(name);
        }

        public async UniTask<UserDisplayNameVO> DecideAsync(CancellationToken token)
        {
            var name = await _displayName.FirstAsync(token);
            return new UserDisplayNameVO(name);
        }

        void IDisposable.Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            _displayName?.Dispose();
        }
    }
}