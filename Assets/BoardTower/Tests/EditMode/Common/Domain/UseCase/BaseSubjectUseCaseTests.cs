using System.Collections.Generic;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    /// <summary>
    /// 抽象クラス BaseSubjectUseCase&lt;T&gt; のテスト。最小限のテスト用サブクラスで検証する。
    /// </summary>
    [TestFixture]
    public sealed class BaseSubjectUseCaseTests
    {
        private sealed class TestEntity : BaseEntity<int>
        {
        }

        private sealed class TestSubjectUseCase : BaseSubjectUseCase<int>
        {
            public TestSubjectUseCase(BaseEntity<int> entity) : base(entity)
            {
            }
        }

        private TestEntity _entity;
        private TestSubjectUseCase _useCase;
        private CompositeDisposable _disposables;

        [SetUp]
        public void SetUp()
        {
            _entity = new TestEntity();
            _useCase = new TestSubjectUseCase(_entity);
            _disposables = new CompositeDisposable();
        }

        [TearDown]
        public void TearDown()
        {
            _disposables.Dispose();
            _useCase.Dispose();
        }

        [Test]
        public void Subject_OnSubscribe_EmitsInitialEntityValue()
        {
            var received = new List<int>();

            _disposables.Add(_useCase.subject.Subscribe(x => received.Add(x)));

            Assert.That(received, Is.EqualTo(new[] { 0 }));
        }

        [Test]
        public void Constructor_WithPresetEntityValue_EmitsPresetValueOnSubscribe()
        {
            _entity.Set(5);
            var useCase = new TestSubjectUseCase(_entity);
            var received = new List<int>();

            _disposables.Add(useCase.subject.Subscribe(x => received.Add(x)));
            useCase.Dispose();

            Assert.That(received, Is.EqualTo(new[] { 5 }));
        }

        [Test]
        public void Set_UpdatesEntityValue()
        {
            _useCase.Set(3);

            Assert.That(_entity.value, Is.EqualTo(3));
        }

        [Test]
        public void Set_NotifiesSubscriberAfterInitialValue()
        {
            var received = new List<int>();
            _disposables.Add(_useCase.subject.Subscribe(x => received.Add(x)));

            _useCase.Set(3);
            _useCase.Set(8);

            Assert.That(received, Is.EqualTo(new[] { 0, 3, 8 }));
        }
    }
}
