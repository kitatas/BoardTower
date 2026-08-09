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
    public sealed class TapScreenPortsTests
    {
        private TapScreenPorts _ports;
        private FakeAsyncPublisher<TapScreenTransitionVO> _publisher;
        private FakeAsyncSubscriber<TapScreenTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<TapScreenTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<TapScreenTransitionVO>();
            _ports = new TapScreenPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsTapScreenTransitionSubscriber()
        {
            Assert.That(_ports.tapScreenTransitionSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishTapScreenAsync_CallsPublisherOnce()
        {
            var vo = TapScreenTransitionVO.Create(Fade.Out);

            await _ports.PublishTapScreenAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishTapScreenAsync_PublishesExactVO()
        {
            var vo = TapScreenTransitionVO.Create(Fade.In);

            await _ports.PublishTapScreenAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task PublishTapScreenAsync_PublishesCorrectFade(Fade fade)
        {
            var vo = TapScreenTransitionVO.Create(fade);

            await _ports.PublishTapScreenAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task PublishTapScreenAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = TapScreenTransitionVO.Create(Fade.Out);

            await _ports.PublishTapScreenAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishTapScreenAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
