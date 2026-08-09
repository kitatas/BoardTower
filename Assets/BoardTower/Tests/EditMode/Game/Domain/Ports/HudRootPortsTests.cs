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
    public sealed class HudRootPortsTests
    {
        private HudRootPorts _ports;
        private FakeAsyncPublisher<HudRootTransitionVO> _publisher;
        private FakeAsyncSubscriber<HudRootTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<HudRootTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<HudRootTransitionVO>();
            _ports = new HudRootPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsHudRootTransitionSubscriber()
        {
            Assert.That(_ports.hudRootTransitionSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishHudRootTransitionAsync_CallsPublisherOnce()
        {
            var vo = HudRootTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishHudRootTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishHudRootTransitionAsync_PublishesExactVO()
        {
            var vo = HudRootTransitionVO.Create(Fade.In, HudRootConfig.FADE_DURATION);

            await _ports.PublishHudRootTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(Fade.In, 0.25f)]
        [TestCase(Fade.Out, 0.0f)]
        [TestCase(Fade.InOut, 0.5f)]
        public async Task PublishHudRootTransitionAsync_PublishesCorrectFadeAndDuration(Fade fade, float duration)
        {
            var vo = HudRootTransitionVO.Create(fade, duration);

            await _ports.PublishHudRootTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(duration));
        }

        [Test]
        public async Task PublishHudRootTransitionAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = HudRootTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishHudRootTransitionAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishHudRootTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
