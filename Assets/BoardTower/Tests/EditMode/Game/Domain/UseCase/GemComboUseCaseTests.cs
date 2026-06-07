using System;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class GemComboUseCaseTests
    {
        private GemComboUseCase _useCase;
        private GemComboEntity _gemComboEntity;
        private PickRelicEntity _pickRelicEntity;

        [SetUp]
        public void SetUp()
        {
            _gemComboEntity = new GemComboEntity();
            _pickRelicEntity = new PickRelicEntity();
            _useCase = new GemComboUseCase(_gemComboEntity, _pickRelicEntity);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Combo_Observable_IsNotNull()
        {
            Assert.That(_useCase.combo, Is.Not.Null);
        }

        [Test]
        public void Constructor_InitialCombo_IsZero()
        {
            int observed = -1;
            _useCase.combo.Subscribe(x => observed = x);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void SetUp_ResetsComboToZero()
        {
            _useCase.Increment();

            _useCase.SetUp();

            int observed = -1;
            _useCase.combo.Subscribe(x => observed = x);
            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Increment_IncreasesComboByOne()
        {
            int observed = -1;
            _useCase.combo.Subscribe(x => observed = x);

            _useCase.Increment();

            Assert.That(observed, Is.EqualTo(1));
        }

        [Test]
        public void Increment_CalledMultipleTimes_AccumulatesCorrectly()
        {
            int observed = -1;
            _useCase.combo.Subscribe(x => observed = x);

            _useCase.Increment();
            _useCase.Increment();
            _useCase.Increment();

            Assert.That(observed, Is.EqualTo(3));
        }

        [Test]
        public void Apply_WithGemType_IncrementsCombo()
        {
            int observed = -1;
            _useCase.combo.Subscribe(x => observed = x);

            _useCase.Apply(SquareEventType.Gem);

            Assert.That(observed, Is.EqualTo(1));
        }

        [Test]
        public void Apply_WithGemType_CalledMultipleTimes_AccumulatesCombo()
        {
            int observed = -1;
            _useCase.combo.Subscribe(x => observed = x);

            _useCase.Apply(SquareEventType.Gem);
            _useCase.Apply(SquareEventType.Gem);

            Assert.That(observed, Is.EqualTo(2));
        }

        [Test]
        public void Apply_WithNonGemType_AndNoRelics_ResetsCombo()
        {
            _useCase.Increment();
            _useCase.Increment();
            int observed = -1;
            _useCase.combo.Subscribe(x => observed = x);

            _useCase.Apply(SquareEventType.Empty);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Apply_WithPlyType_AndNoRelics_ResetsCombo()
        {
            _useCase.Increment();
            int observed = -1;
            _useCase.combo.Subscribe(x => observed = x);

            _useCase.Apply(SquareEventType.Ply);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Apply_WithBlockType_AndNoRelics_ResetsCombo()
        {
            _useCase.Increment();
            int observed = -1;
            _useCase.combo.Subscribe(x => observed = x);

            _useCase.Apply(SquareEventType.Block);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
