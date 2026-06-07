using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BoardTower.Tests.EditMode.TestHelpers
{
    internal sealed class FakeAsyncPublisher<T> : IAsyncPublisher<T>
    {
        public T LastPublished { get; private set; }
        public int PublishCount { get; private set; }

        public void Publish(T message, CancellationToken cancellationToken = new CancellationToken())
        {
        }

        public UniTask PublishAsync(T message, CancellationToken cancellationToken = default)
        {
            LastPublished = message;
            PublishCount++;
            return UniTask.CompletedTask;
        }

        public UniTask PublishAsync(T message, AsyncPublishStrategy publishStrategy,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return default;
        }
    }

    internal sealed class FakeAsyncSubscriber<T> : IAsyncSubscriber<T>
    {
        public IDisposable Subscribe(IAsyncMessageHandler<T> handler, params AsyncMessageHandlerFilter<T>[] filters)
        {
            return NopDisposable.Instance;
        }
    }

    internal sealed class NopDisposable : IDisposable
    {
        public static readonly NopDisposable Instance = new NopDisposable();
        public void Dispose() { }
    }
}
