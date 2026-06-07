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
    public sealed class ExceptionPortsTests
    {
        private ExceptionPorts _ports;
        private FakeAsyncPublisher<ExceptionNotifyVO> _notifyPublisher;
        private FakeAsyncPublisher<ExceptionActionVO> _actionPublisher;
        private FakeAsyncSubscriber<ExceptionNotifyVO> _notifySubscriber;
        private FakeAsyncSubscriber<ExceptionActionVO> _actionSubscriber;

        [SetUp]
        public void SetUp()
        {
            _notifyPublisher = new FakeAsyncPublisher<ExceptionNotifyVO>();
            _actionPublisher = new FakeAsyncPublisher<ExceptionActionVO>();
            _notifySubscriber = new FakeAsyncSubscriber<ExceptionNotifyVO>();
            _actionSubscriber = new FakeAsyncSubscriber<ExceptionActionVO>();
            _ports = new ExceptionPorts(_notifySubscriber, _actionSubscriber, _notifyPublisher, _actionPublisher);
        }

        [Test]
        public void Constructor_AssignsExceptionNotifySubscriber()
        {
            Assert.That(_ports.exceptionNotifySubscriber, Is.EqualTo(_notifySubscriber));
        }

        [Test]
        public void Constructor_AssignsExceptionActionSubscriber()
        {
            Assert.That(_ports.exceptionActionSubscriber, Is.EqualTo(_actionSubscriber));
        }

        [Test]
        public async Task PublishExceptionNotifyAsync_CallsPublisherOnce()
        {
            var vo = ExceptionNotifyVO.Create(new QuitExceptionVO("msg"), Fade.Out, 0.0f);

            await _ports.PublishExceptionNotifyAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_notifyPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishExceptionNotifyAsync_PublishesExactVO()
        {
            var vo = ExceptionNotifyVO.Create(new QuitExceptionVO("msg"), Fade.Out, 0.0f);

            await _ports.PublishExceptionNotifyAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_notifyPublisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(Fade.In, 0.25f)]
        [TestCase(Fade.Out, 0.0f)]
        [TestCase(Fade.InOut, 0.5f)]
        public async Task PublishExceptionNotifyAsync_PublishesCorrectFadeAndDuration(Fade fade, float duration)
        {
            var vo = ExceptionNotifyVO.Create(new QuitExceptionVO("msg"), fade, duration);

            await _ports.PublishExceptionNotifyAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_notifyPublisher.LastPublished.transition.fade, Is.EqualTo(fade));
            Assert.That(_notifyPublisher.LastPublished.transition.duration, Is.EqualTo(duration));
        }

        [Test]
        public async Task PublishExceptionNotifyAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = ExceptionNotifyVO.Create(new QuitExceptionVO("msg"), Fade.Out, 0.0f);

            await _ports.PublishExceptionNotifyAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishExceptionNotifyAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_notifyPublisher.PublishCount, Is.EqualTo(2));
        }

        [Test]
        public async Task PublishExceptionActionAsync_CallsPublisherOnce()
        {
            var vo = new ExceptionActionVO(new RetryExceptionVO("retry"));

            await _ports.PublishExceptionActionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_actionPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishExceptionActionAsync_PublishesExactVO()
        {
            var vo = new ExceptionActionVO(new RetryExceptionVO("retry"));

            await _ports.PublishExceptionActionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_actionPublisher.LastPublished, Is.EqualTo(vo));
        }

        [Test]
        public async Task PublishExceptionActionAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = new ExceptionActionVO(new RebootExceptionVO("reboot"));

            await _ports.PublishExceptionActionAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishExceptionActionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_actionPublisher.PublishCount, Is.EqualTo(2));
        }
    }
}
