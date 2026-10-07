using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using MessagePipe;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Domain.Ports
{
    /// <summary>
    /// 抽象クラス BasePubSubPorts&lt;T&gt; のテスト。最小限のテスト用サブクラスで検証する。
    /// </summary>
    [TestFixture]
    public sealed class BasePubSubPortsTests
    {
        private sealed class TestPorts : BasePubSubPorts<int>
        {
            public TestPorts(IAsyncSubscriber<int> subscriber, IAsyncPublisher<int> publisher) : base(subscriber,
                publisher)
            {
            }
        }

        private TestPorts _ports;
        private FakeAsyncPublisher<int> _publisher;
        private FakeAsyncSubscriber<int> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<int>();
            _subscriber = new FakeAsyncSubscriber<int>();
            _ports = new TestPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsSubscriber()
        {
            Assert.That(_ports.subscriber, Is.SameAs(_subscriber));
        }

        [Test]
        public async Task PublishAsync_CallsPublisherOnce()
        {
            await _ports.PublishAsync(1, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishAsync_PublishesGivenMessage()
        {
            await _ports.PublishAsync(42, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(42));
        }
    }
}
