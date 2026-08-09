using System.Collections.Generic;
using BoardTower.Common.Data.Entity;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class AchievementUseCaseTests
    {
        private AchievementUseCase _useCase;
        private UserEntity _userEntity;
        private FakeAsyncPublisher<IEnumerable<AchievementContentVO>> _publisher;
        private FakeAsyncSubscriber<IEnumerable<AchievementContentVO>> _subscriber;
        private AchievementPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _userEntity = new UserEntity();
            _publisher = new FakeAsyncPublisher<IEnumerable<AchievementContentVO>>();
            _subscriber = new FakeAsyncSubscriber<IEnumerable<AchievementContentVO>>();
            _ports = new AchievementPorts(_subscriber, _publisher);

            // AchievementRepository / LocaleRepository は MasterMemory/Localization 依存のため null を渡す
            _useCase = new AchievementUseCase(_userEntity, _ports, null, null);
        }

        [Test]
        public void AchievementContents_Property_ReturnsAchievementContentsSubscriber()
        {
            Assert.That(_useCase.achievementContents, Is.EqualTo(_subscriber));
        }
    }
}
