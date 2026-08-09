using System;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class ScoreUseCaseTests
    {
        private ScoreUseCase _useCase;
        private GemComboEntity _gemComboEntity;
        private PickRelicEntity _pickRelicEntity;
        private RoundEntity _roundEntity;
        private ScoreEntity _scoreEntity;

        [SetUp]
        public void SetUp()
        {
            _gemComboEntity = new GemComboEntity();
            _pickRelicEntity = new PickRelicEntity();
            _roundEntity = new RoundEntity();
            _scoreEntity = new ScoreEntity();

            // ScoreRateRepository は MasterMemory 依存のため null を渡す
            _useCase = new ScoreUseCase(_gemComboEntity, _pickRelicEntity, _roundEntity, _scoreEntity, null);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Score_Observable_IsNotNull()
        {
            Assert.That(_useCase.score, Is.Not.Null);
        }

        [Test]
        public void Constructor_InitialScore_IsZero()
        {
            int observed = -1;
            _useCase.score.Subscribe(x => observed = x);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Init_ResetsScoreToZero()
        {
            _scoreEntity.Add(100);

            _useCase.Init();

            int observed = -1;
            _useCase.score.Subscribe(x => observed = x);
            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Init_PublishesZeroValue()
        {
            _scoreEntity.Add(50);
            int observed = -1;
            _useCase.score.Subscribe(x => observed = x);

            _useCase.Init();

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void ApplyRideOnSquare_WithNonCollapseType_DoesNotChangeScore()
        {
            _useCase.Init();
            int observed = -1;
            _useCase.score.Subscribe(x => observed = x);

            _useCase.ApplyRideOnSquare(BoardTower.Game.Application.SquareEventType.Gem);

            Assert.That(observed, Is.EqualTo(0));
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
