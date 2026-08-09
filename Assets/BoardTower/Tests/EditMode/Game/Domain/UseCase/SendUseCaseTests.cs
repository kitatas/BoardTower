using BoardTower.Common.Data.Entity;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.UseCase;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class SendUseCaseTests
    {
        private SendUseCase _useCase;
        private ScoreEntity _scoreEntity;
        private UserEntity _userEntity;

        [SetUp]
        public void SetUp()
        {
            _scoreEntity = new ScoreEntity();
            _userEntity = new UserEntity();

            // PlayFabRepository は外部ネットワーク依存のため null を渡す
            _useCase = new SendUseCase(_scoreEntity, _userEntity, null);
        }

        [Test]
        public void Constructor_WithValidDependencies_DoesNotThrow()
        {
            Assert.That(() => new SendUseCase(_scoreEntity, _userEntity, null), Throws.Nothing);
        }

        [Test]
        public void ScoreEntity_InitialValue_IsZero()
        {
            Assert.That(_scoreEntity.value, Is.EqualTo(0));
        }

        [Test]
        public void ScoreEntity_AfterAdd_ReflectsUpdatedValue()
        {
            _scoreEntity.Add(500);

            Assert.That(_scoreEntity.value, Is.EqualTo(500));
        }
    }
}
