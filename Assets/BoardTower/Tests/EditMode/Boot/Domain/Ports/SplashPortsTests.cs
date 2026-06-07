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
    public sealed class SplashPortsTests
    {
        private SplashPorts _ports;
        private FakeAsyncPublisher<SplashTransitionVO> _publisher;
        private FakeAsyncSubscriber<SplashTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<SplashTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<SplashTransitionVO>();
            _ports = new SplashPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsSplashTransitionSubscriber()
        {
            Assert.That(_ports.splashTransitionSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishSplashTransitionAsync_CallsPublisherOnce()
        {
            var vo = SplashTransitionVO.Create(null, Fade.Out, 0.0f);

            await _ports.PublishSplashTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishSplashTransitionAsync_PublishesExactVO()
        {
            var vo = SplashTransitionVO.Create(null, Fade.Out, 0.0f);

            await _ports.PublishSplashTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(Fade.Out, 0.0f)]
        [TestCase(Fade.Out, 0.5f)]
        public async Task PublishSplashTransitionAsync_PublishesCorrectFadeAndDuration(Fade fade, float duration)
        {
            var vo = SplashTransitionVO.Create(null, fade, duration);

            await _ports.PublishSplashTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(duration));
        }

        [Test]
        public async Task PublishSplashTransitionAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = SplashTransitionVO.Create(null, Fade.Out, 0.0f);

            await _ports.PublishSplashTransitionAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishSplashTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
