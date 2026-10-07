using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Ports;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using MessagePipe;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    /// <summary>
    /// 抽象クラス BasePubSubUseCase&lt;T&gt; のテスト。最小限のテスト用サブクラスで検証する。
    /// </summary>
    [TestFixture]
    public sealed class BasePubSubUseCaseTests
    {
        private sealed class TestEntity : BaseEntity<int>
        {
        }

        // Set で値を加工するEntity（UseCase が入力値ではなく entity.value を publish することの検証用）
        private sealed class DoublingEntity : BaseEntity<int>
        {
            public override void Set(int t) => value = t * 2;
        }

        private sealed class TestPorts : BasePubSubPorts<int>
        {
            public TestPorts(IAsyncSubscriber<int> subscriber, IAsyncPublisher<int> publisher) : base(subscriber,
                publisher)
            {
            }
        }

        private sealed class TestUseCase : BasePubSubUseCase<int>
        {
            public TestUseCase(BaseEntity<int> entity, BasePubSubPorts<int> ports) : base(entity, ports)
            {
            }
        }

        private FakeAsyncPublisher<int> _publisher;
        private FakeAsyncSubscriber<int> _subscriber;
        private TestPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<int>();
            _subscriber = new FakeAsyncSubscriber<int>();
            _ports = new TestPorts(_subscriber, _publisher);
        }

        [Test]
        public void Subscriber_ReturnsPortsSubscriber()
        {
            var useCase = new TestUseCase(new TestEntity(), _ports);

            Assert.That(useCase.subscriber, Is.SameAs(_subscriber));
        }

        [Test]
        public async Task PublishAsync_SetsValueToEntity()
        {
            var entity = new TestEntity();
            var useCase = new TestUseCase(entity, _ports);

            await useCase.PublishAsync(7, CancellationToken.None).AsTask();

            Assert.That(entity.value, Is.EqualTo(7));
        }

        [Test]
        public async Task PublishAsync_PublishesValueThroughPorts()
        {
            var useCase = new TestUseCase(new TestEntity(), _ports);

            await useCase.PublishAsync(7, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(7));
        }

        [Test]
        public async Task PublishAsync_CallsPublisherOnce()
        {
            var useCase = new TestUseCase(new TestEntity(), _ports);

            await useCase.PublishAsync(7, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishAsync_WhenEntityTransformsValue_PublishesEntityValue()
        {
            var useCase = new TestUseCase(new DoublingEntity(), _ports);

            await useCase.PublishAsync(5, CancellationToken.None).AsTask();

            // Entity 保持値 (10) が publish される。入力値 (5) ではない
            Assert.That(_publisher.LastPublished, Is.EqualTo(10));
        }
    }
}
