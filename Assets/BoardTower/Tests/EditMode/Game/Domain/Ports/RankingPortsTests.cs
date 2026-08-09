using System.Linq;
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
    public sealed class RankingPortsTests
    {
        private RankingPorts _ports;
        private FakeAsyncPublisher<ScoreRankingVO> _publisher;
        private FakeAsyncSubscriber<ScoreRankingVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<ScoreRankingVO>();
            _subscriber = new FakeAsyncSubscriber<ScoreRankingVO>();
            _ports = new RankingPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsScoreRankingSubscriber()
        {
            Assert.That(_ports.scoreRankingSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishScoreRankingAsync_CallsPublisherOnce()
        {
            var vo = new ScoreRankingVO(Enumerable.Empty<PlayFabRankingEntryVO>());

            await _ports.PublishScoreRankingAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishScoreRankingAsync_PublishesExactVO()
        {
            var vo = new ScoreRankingVO(Enumerable.Empty<PlayFabRankingEntryVO>());

            await _ports.PublishScoreRankingAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [Test]
        public async Task PublishScoreRankingAsync_PublishesVOWithCorrectEntries()
        {
            var entries = new[]
            {
                new PlayFabRankingEntryVO("id1", 1, "Player1", "1000"),
                new PlayFabRankingEntryVO("id2", 2, "Player2", "500"),
            };
            var vo = new ScoreRankingVO(entries);

            await _ports.PublishScoreRankingAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.entries.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task PublishScoreRankingAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = new ScoreRankingVO(Enumerable.Empty<PlayFabRankingEntryVO>());

            await _ports.PublishScoreRankingAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishScoreRankingAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
