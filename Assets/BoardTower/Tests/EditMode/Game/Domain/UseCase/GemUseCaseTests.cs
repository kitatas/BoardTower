using System;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class GemUseCaseTests
    {
        private GemUseCase _useCase;
        private GemEntity _gemEntity;

        [SetUp]
        public void SetUp()
        {
            _gemEntity = new GemEntity();
            _useCase = new GemUseCase(_gemEntity);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Gem_Observable_IsNotNull()
        {
            Assert.That(_useCase.gem, Is.Not.Null);
        }

        [Test]
        public void SetUp_ResetsGemToZero()
        {
            _gemEntity.Add(5);

            _useCase.SetUp();

            int result = 0;
            _useCase.gem.Subscribe(x => result = x);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void SetUp_PublishesZeroValue()
        {
            int observed = -1;
            _useCase.gem.Subscribe(x => observed = x);

            _useCase.SetUp();

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Add_IncreasesGemValue()
        {
            _useCase.SetUp();
            int observed = -1;
            _useCase.gem.Subscribe(x => observed = x);

            _useCase.Add(3);

            Assert.That(observed, Is.EqualTo(3));
        }

        [Test]
        public void Add_CalledMultipleTimes_AccumulatesCorrectly()
        {
            _useCase.SetUp();
            int observed = -1;
            _useCase.gem.Subscribe(x => observed = x);

            _useCase.Add(2);
            _useCase.Add(3);

            Assert.That(observed, Is.EqualTo(5));
        }

        [Test]
        public void Add_WithZero_DoesNotChangeValue()
        {
            _useCase.SetUp();
            int observed = -1;
            _useCase.gem.Subscribe(x => observed = x);

            _useCase.Add(0);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void SetUp_AfterAdd_ResetsToZero()
        {
            _useCase.SetUp();
            _useCase.Add(5);

            _useCase.SetUp();

            int observed = -1;
            _useCase.gem.Subscribe(x => observed = x);
            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
