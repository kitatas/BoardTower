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
    public sealed class LotRelicPortsTests
    {
        private LotRelicPorts _ports;
        private FakeAsyncPublisher<LotRelicTransitionVO> _publisher;
        private FakeAsyncSubscriber<LotRelicTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<LotRelicTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<LotRelicTransitionVO>();
            _ports = new LotRelicPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsLotRelicTransitionSubscriber()
        {
            Assert.That(_ports.lotRelicTransitionSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishLotRelicTransitionAsync_CallsPublisherOnce()
        {
            var vo = LotRelicTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishLotRelicTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishLotRelicTransitionAsync_PublishesExactVO()
        {
            var vo = LotRelicTransitionVO.Create(Fade.In, RelicConfig.LOT_FADE_DURATION);

            await _ports.PublishLotRelicTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(Fade.In, 0.25f)]
        [TestCase(Fade.Out, 0.0f)]
        public async Task PublishLotRelicTransitionAsync_PublishesCorrectFadeAndDuration(Fade fade, float duration)
        {
            var vo = LotRelicTransitionVO.Create(fade, duration);

            await _ports.PublishLotRelicTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(duration));
        }

        [Test]
        public async Task PublishLotRelicTransitionAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = LotRelicTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishLotRelicTransitionAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishLotRelicTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
