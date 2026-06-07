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
    public sealed class DisplayNamePortsTests
    {
        private DisplayNamePorts _ports;
        private FakeAsyncPublisher<DisplayNameTransitionVO> _publisher;
        private FakeAsyncSubscriber<DisplayNameTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<DisplayNameTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<DisplayNameTransitionVO>();
            _ports = new DisplayNamePorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsDisplayNameTransitionSubscriber()
        {
            Assert.That(_ports.displayNameTransitionSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishDisplayNameTransitionAsync_CallsPublisherOnce()
        {
            var vo = DisplayNameTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishDisplayNameTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishDisplayNameTransitionAsync_PublishesExactVO()
        {
            var vo = DisplayNameTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishDisplayNameTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(Fade.In, 0.25f)]
        [TestCase(Fade.Out, 0.0f)]
        [TestCase(Fade.InOut, 0.5f)]
        public async Task PublishDisplayNameTransitionAsync_PublishesCorrectFadeAndDuration(Fade fade, float duration)
        {
            var vo = DisplayNameTransitionVO.Create(fade, duration);

            await _ports.PublishDisplayNameTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(duration));
        }

        [Test]
        public async Task PublishDisplayNameTransitionAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = DisplayNameTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishDisplayNameTransitionAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishDisplayNameTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
