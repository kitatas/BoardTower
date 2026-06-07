using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Domain.Ports
{
    [TestFixture]
    public sealed class LoadingPortsTests
    {
        private LoadingPorts _ports;
        private FakeAsyncPublisher<LoadingTransitionVO> _publisher;
        private FakeAsyncSubscriber<LoadingTransitionVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<LoadingTransitionVO>();
            _subscriber = new FakeAsyncSubscriber<LoadingTransitionVO>();
            _ports = new LoadingPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsLoadingTransitionSubscriber()
        {
            Assert.That(_ports.loadingTransitionSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishLoadingTransitionAsync_CallsPublisherOnce()
        {
            var vo = LoadingTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishLoadingTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishLoadingTransitionAsync_PublishesExactVO()
        {
            var vo = LoadingTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishLoadingTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(Fade.In, 0.25f)]
        [TestCase(Fade.Out, 0.0f)]
        [TestCase(Fade.InOut, 0.5f)]
        public async Task PublishLoadingTransitionAsync_PublishesCorrectFadeAndDuration(Fade fade, float duration)
        {
            var vo = LoadingTransitionVO.Create(fade, duration);

            await _ports.PublishLoadingTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(duration));
        }

        [Test]
        public async Task PublishLoadingTransitionAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = LoadingTransitionVO.Create(Fade.Out, 0.0f);

            await _ports.PublishLoadingTransitionAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishLoadingTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
