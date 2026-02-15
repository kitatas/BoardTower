using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace BoardTower.Common.Utility
{
    public sealed class AsyncLockLite : IDisposable
    {
        private readonly SemaphoreSlim _sem = new(1, 1);

        public async UniTask<Releaser> LockAsync(CancellationToken ct)
        {
            await _sem.WaitAsync(ct);
            return new Releaser(_sem);
        }

        public void Dispose() => _sem.Dispose();

        public readonly struct Releaser : IDisposable
        {
            private readonly SemaphoreSlim _sem;
            public Releaser(SemaphoreSlim sem) => _sem = sem;
            public void Dispose() => _sem?.Release();
        }
    }
}