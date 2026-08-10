using BoardTower.Common.Data.Entity;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class RankingUseCaseTests
    {
        private RankingUseCase _useCase;
        private FakeAsyncPublisher<ScoreRankingVO> _publisher;
        private FakeAsyncSubscriber<ScoreRankingVO> _subscriber;
        private RankingPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<ScoreRankingVO>();
            _subscriber = new FakeAsyncSubscriber<ScoreRankingVO>();
            _ports = new RankingPorts(_subscriber, _publisher);

            _useCase = new RankingUseCase(new UserEntity(), _ports, null);
        }

        [Test]
        public void ScoreRanking_Property_ReturnsScoreRankingSubscriber()
        {
            Assert.That(_useCase.scoreRanking, Is.EqualTo(_subscriber));
        }
    }
}
