using System;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class RoundClearUseCaseTests
    {
        private RoundClearUseCase _useCase;
        private GemEntity _gemEntity;
        private RoundClearGemEntity _roundClearGemEntity;
        private PickRelicEntity _pickRelicEntity;

        [SetUp]
        public void SetUp()
        {
            _gemEntity = new GemEntity();
            _roundClearGemEntity = new RoundClearGemEntity();
            _pickRelicEntity = new PickRelicEntity();

            // RoundRepository requires MemoryDatabase, so pass round=0 in tests
            // to bypass repository lookup (round > 0 ? repo.Find(round) : 0)
            _useCase = new RoundClearUseCase(_gemEntity, _pickRelicEntity, _roundClearGemEntity, null);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void RoundClear_Observable_IsNotNull()
        {
            Assert.That(_useCase.roundClear, Is.Not.Null);
        }

        [Test]
        public void SetUp_WithRoundZero_SetsRoundClearGemToZero()
        {
            int observed = -1;
            _useCase.roundClear.Subscribe(x => observed = x);

            _useCase.SetUp(0);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void IsClear_WhenGemEqualsRoundClearGem_ReturnsTrue()
        {
            _useCase.SetUp(0);
            _gemEntity.Set(0);

            Assert.That(_useCase.IsClear(), Is.True);
        }

        [Test]
        public void IsClear_WhenGemExceedsRoundClearGem_ReturnsTrue()
        {
            _useCase.SetUp(0);
            _gemEntity.Add(5);

            Assert.That(_useCase.IsClear(), Is.True);
        }

        [Test]
        public void IsClear_WhenGemBelowRoundClearGem_ReturnsFalse()
        {
            _roundClearGemEntity.Set(5);
            _gemEntity.Set(3);

            Assert.That(_useCase.IsClear(), Is.False);
        }

        [Test]
        public void IsOverflowClearGem_WhenGemEqualsRoundClearGem_ReturnsFalse()
        {
            _useCase.SetUp(0);
            _gemEntity.Set(0);

            Assert.That(_useCase.IsOverflowClearGem(), Is.False);
        }

        [Test]
        public void IsOverflowClearGem_WhenGemExceedsRoundClearGem_ReturnsTrue()
        {
            _useCase.SetUp(0);
            _gemEntity.Add(1);

            Assert.That(_useCase.IsOverflowClearGem(), Is.True);
        }

        [Test]
        public void IsOverflowClearGem_WhenGemBelowRoundClearGem_ReturnsFalse()
        {
            _roundClearGemEntity.Set(5);
            _gemEntity.Set(3);

            Assert.That(_useCase.IsOverflowClearGem(), Is.False);
        }

        [Test]
        public void IsClear_WhenRoundClearGemIsZeroAndGemIsZero_ReturnsTrue()
        {
            Assert.That(_useCase.IsClear(), Is.True);
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
