using System;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class RoundUseCaseTests
    {
        private RoundUseCase _useCase;
        private RoundEntity _roundEntity;

        [SetUp]
        public void SetUp()
        {
            _roundEntity = new RoundEntity();
            _useCase = new RoundUseCase(_roundEntity);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Round_Observable_IsNotNull()
        {
            Assert.That(_useCase.round, Is.Not.Null);
        }

        [Test]
        public void Constructor_InitialRound_IsZero()
        {
            int observed = -1;
            _useCase.round.Subscribe(x => observed = x);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Init_ResetsRoundToZero()
        {
            _useCase.Increment();
            _useCase.Increment();

            _useCase.Init();

            int observed = -1;
            _useCase.round.Subscribe(x => observed = x);
            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Increment_IncreasesRoundByOne()
        {
            int observed = -1;
            _useCase.round.Subscribe(x => observed = x);

            _useCase.Increment();

            Assert.That(observed, Is.EqualTo(1));
        }

        [Test]
        public void Increment_CalledMultipleTimes_AccumulatesCorrectly()
        {
            int observed = -1;
            _useCase.round.Subscribe(x => observed = x);

            _useCase.Increment();
            _useCase.Increment();
            _useCase.Increment();

            Assert.That(observed, Is.EqualTo(3));
        }

        [Test]
        public void IsMaxRound_WhenBelowMax_ReturnsFalse()
        {
            _roundEntity.Set(RoundConfig.MAX_NUM - 1);

            Assert.That(_useCase.IsMaxRound(), Is.False);
        }

        [Test]
        public void IsMaxRound_WhenAtMax_ReturnsTrue()
        {
            _roundEntity.Set(RoundConfig.MAX_NUM);

            Assert.That(_useCase.IsMaxRound(), Is.True);
        }

        [Test]
        public void IsMaxRound_WhenAboveMax_ReturnsFalse()
        {
            _roundEntity.Set(RoundConfig.MAX_NUM + 1);

            Assert.That(_useCase.IsMaxRound(), Is.False);
        }

        [Test]
        public void IsMaxRound_AtInitialValue_ReturnsFalse()
        {
            Assert.That(_useCase.IsMaxRound(), Is.False);
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
