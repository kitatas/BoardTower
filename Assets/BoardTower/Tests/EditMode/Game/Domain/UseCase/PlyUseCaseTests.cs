using System;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class PlyUseCaseTests
    {
        private PlyUseCase _useCase;
        private PlyEntity _plyEntity;
        private PickRelicEntity _pickRelicEntity;

        [SetUp]
        public void SetUp()
        {
            _plyEntity = new PlyEntity();
            _pickRelicEntity = new PickRelicEntity();

            // RoundRepository requires MemoryDatabase, so pass round=0 in tests
            // to bypass repository lookup (round > 0 ? repo.Find(round) : 0)
            _useCase = new PlyUseCase(_pickRelicEntity, _plyEntity, null);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Ply_Observable_IsNotNull()
        {
            Assert.That(_useCase.ply, Is.Not.Null);
        }

        [Test]
        public void PlyMax_Observable_IsNotNull()
        {
            Assert.That(_useCase.plyMax, Is.Not.Null);
        }

        [Test]
        public void SetUp_WithRoundZero_SetsPlyToZero()
        {
            int observed = -1;
            _useCase.ply.Subscribe(x => observed = x);

            _useCase.SetUp(0);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void SetUp_WithRoundZero_SetsPlyMaxToZero()
        {
            int observed = -1;
            _useCase.plyMax.Subscribe(x => observed = x);

            _useCase.SetUp(0);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Add_IncreasesplyValue()
        {
            _plyEntity.SetUp(10);
            _plyEntity.Set(3);
            int observed = -1;
            _useCase.ply.Subscribe(x => observed = x);

            _useCase.Add(2);

            Assert.That(observed, Is.EqualTo(5));
        }

        [Test]
        public void Decrease_DecreasesplyByOne()
        {
            _plyEntity.SetUp(5);
            int observed = -1;
            _useCase.ply.Subscribe(x => observed = x);

            _useCase.Decrease();

            Assert.That(observed, Is.EqualTo(4));
        }

        [Test]
        public void Decrease_FromZero_StaysAtZero()
        {
            _plyEntity.SetUp(0);
            int observed = -1;
            _useCase.ply.Subscribe(x => observed = x);

            _useCase.Decrease();

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void IsZero_WhenPlyIsZero_ReturnsTrue()
        {
            _plyEntity.SetUp(5);
            _plyEntity.Set(0);

            Assert.That(_useCase.IsZero(), Is.True);
        }

        [Test]
        public void IsZero_WhenPlyIsAboveZero_ReturnsFalse()
        {
            _plyEntity.SetUp(5);

            Assert.That(_useCase.IsZero(), Is.False);
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
