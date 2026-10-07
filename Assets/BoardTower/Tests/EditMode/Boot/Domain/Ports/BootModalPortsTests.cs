using System.Threading;
using System.Threading.Tasks;
using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Common.Application;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Domain.Ports
{
    [TestFixture]
    public sealed class BootModalPortsTests
    {
        private BootModalPorts _ports;
        private FakeAsyncPublisher<BaseModalTransitionVO<BootModalType>> _publisher;
        private FakeAsyncSubscriber<BaseModalTransitionVO<BootModalType>> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<BaseModalTransitionVO<BootModalType>>();
            _subscriber = new FakeAsyncSubscriber<BaseModalTransitionVO<BootModalType>>();
            _ports = new BootModalPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsSubscriber()
        {
            Assert.That(_ports.subscriber, Is.SameAs(_subscriber));
        }

        [Test]
        public async Task PublishAsync_CallsPublisherOnce()
        {
            var vo = BootModalTransitionVO.Create(BootModalType.Update, Fade.In, 0.25f);

            await _ports.PublishAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishAsync_PublishesExactVO()
        {
            var vo = BootModalTransitionVO.Create(BootModalType.Name, Fade.Out, 0.25f);

            await _ports.PublishAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.SameAs(vo));
        }
    }
}
