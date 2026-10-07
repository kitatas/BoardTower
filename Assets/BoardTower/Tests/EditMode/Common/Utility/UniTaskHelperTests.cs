using System;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Utility;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Utility
{
    /// <summary>
    /// UniTaskHelper (DelayAsync / AsyncLockLite) のテスト。
    /// BuildLinkedTokenSource は MonoBehaviour の破棄検知 (PlayMode 相当) に依存するため対象外。
    /// </summary>
    [TestFixture]
    public sealed class UniTaskHelperTests
    {
        private AsyncLockLite _lock;

        [SetUp]
        public void SetUp()
        {
            _lock = new AsyncLockLite();
        }

        [TearDown]
        public void TearDown()
        {
            _lock.Dispose();
        }

        [Test]
        public void DelayAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.CatchAsync<OperationCanceledException>(async () =>
                await UniTaskHelper.DelayAsync(1.0f, cts.Token).AsTask());
        }

        [Test]
        public async Task LockAsync_WhenUnlocked_CompletesImmediately()
        {
            var task = _lock.LockAsync(CancellationToken.None);

            Assert.That(task.Status, Is.EqualTo(UniTaskStatus.Succeeded));

            (await task.AsTask()).Dispose();
        }

        [Test]
        public async Task LockAsync_WhenAlreadyHeld_StaysPendingUntilReleased()
        {
            var first = await _lock.LockAsync(CancellationToken.None).AsTask();

            var second = _lock.LockAsync(CancellationToken.None);

            Assert.That(second.Status, Is.EqualTo(UniTaskStatus.Pending));

            // 後続の待機が残らないよう、取得済みロックを解放しておく
            first.Dispose();
        }

        [Test]
        public async Task LockAsync_AfterReleaserDisposed_CompletesImmediately()
        {
            var first = await _lock.LockAsync(CancellationToken.None).AsTask();
            first.Dispose();

            var second = _lock.LockAsync(CancellationToken.None);

            Assert.That(second.Status, Is.EqualTo(UniTaskStatus.Succeeded));

            (await second.AsTask()).Dispose();
        }

        [Test]
        public void LockAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.CatchAsync<OperationCanceledException>(async () =>
                await _lock.LockAsync(cts.Token).AsTask());
        }

        [Test]
        public void Releaser_DefaultInstance_DisposeDoesNotThrow()
        {
            var releaser = default(AsyncLockLite.Releaser);

            Assert.That(() => releaser.Dispose(), Throws.Nothing);
        }
    }
}
