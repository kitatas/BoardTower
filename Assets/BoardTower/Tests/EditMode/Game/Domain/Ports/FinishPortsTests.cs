using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class FinishPortsTests
    {
        private FinishPorts _ports;
        private FakeAsyncPublisher<FinishTransitionVO> _publisher;
        private FakeAsyncSubscriber<FinishTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<FinishTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<FinishTransitionVO>();
            _ports = new FinishPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsFinishTransitionSubscriber()
        {
            Assert.That(_ports.finishTransitionSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishFinishAsync_CallsPublisherOnce()
        {
            var vo = FinishTransitionVO.Create(FinishType.Clear, Fade.Out, 0.0f);

            await _ports.PublishFinishAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishFinishAsync_PublishesExactVO()
        {
            var vo = FinishTransitionVO.Create(FinishType.Fail, Fade.In, 0.25f);

            await _ports.PublishFinishAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(FinishType.Clear, Fade.In)]
        [TestCase(FinishType.Fail, Fade.Out)]
        public async Task PublishFinishAsync_PublishesCorrectTypeAndFade(FinishType type, Fade fade)
        {
            var vo = FinishTransitionVO.Create(type, fade, 0.25f);

            await _ports.PublishFinishAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.type, Is.EqualTo(type));
            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task PublishFinishAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = FinishTransitionVO.Create(FinishType.Clear, Fade.Out, 0.0f);

            await _ports.PublishFinishAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishFinishAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
