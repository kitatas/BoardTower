using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class AchievementPortsTests
    {
        private AchievementPorts _ports;
        private FakeAsyncPublisher<IEnumerable<AchievementContentVO>> _publisher;
        private FakeAsyncSubscriber<IEnumerable<AchievementContentVO>> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<IEnumerable<AchievementContentVO>>();
            _subscriber = new FakeAsyncSubscriber<IEnumerable<AchievementContentVO>>();
            _ports = new AchievementPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsAchievementContentsSubscriber()
        {
            Assert.That(_ports.achievementContentsSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishAchievementContentsAsync_CallsPublisherOnce()
        {
            var contents = new List<AchievementContentVO>();

            await _ports.PublishAchievementContentsAsync(contents, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishAchievementContentsAsync_PublishesExactContents()
        {
            var contents = new List<AchievementContentVO>();

            await _ports.PublishAchievementContentsAsync(contents, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(contents));
        }

        [Test]
        public async Task PublishAchievementContentsAsync_CalledMultipleTimes_IncrementsCount()
        {
            var contents = new List<AchievementContentVO>();

            await _ports.PublishAchievementContentsAsync(contents, CancellationToken.None).AsTask();
            await _ports.PublishAchievementContentsAsync(contents, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
