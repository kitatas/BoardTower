using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Ports;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using MessagePipe;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    /// <summary>
    /// 抽象クラス BaseModalUseCase&lt;T&gt; のテスト。最小限のテスト用サブクラスで検証する。
    /// </summary>
    [TestFixture]
    public sealed class BaseModalUseCaseTests
    {
        // NOTE: public テストメソッドの引数に使うため public にする（アクセシビリティ不整合の回避）
        public enum TestModal
        {
            None,
            First,
            Second,
        }

        private const float TRANSITION_DURATION = 0.25f;

        private sealed class TestModalVO : BaseModalVO<TestModal>
        {
            public TestModalVO(TestModal type, Fade fade) : base(type, fade)
            {
            }
        }

        private sealed class TestModalTransitionVO : BaseModalTransitionVO<TestModal>
        {
            public TestModalTransitionVO(TestModal type, TransitionVO transition) : base(type, transition)
            {
            }
        }

        private sealed class TestModalEntity : BaseEntity<BaseModalTransitionVO<TestModal>>
        {
        }

        private sealed class TestModalPorts : BasePubSubPorts<BaseModalTransitionVO<TestModal>>
        {
            public TestModalPorts(IAsyncSubscriber<BaseModalTransitionVO<TestModal>> subscriber,
                IAsyncPublisher<BaseModalTransitionVO<TestModal>> publisher) : base(subscriber, publisher)
            {
            }
        }

        private sealed class TestModalUseCase : BaseModalUseCase<TestModal>
        {
            public TestModalUseCase(BaseEntity<BaseModalTransitionVO<TestModal>> entity,
                BasePubSubPorts<BaseModalTransitionVO<TestModal>> ports) : base(entity, ports)
            {
            }

            protected override BaseModalTransitionVO<TestModal> GetModalTransition(BaseModalVO<TestModal> modal)
            {
                return new TestModalTransitionVO(modal.type, new TransitionVO(modal.fade, TRANSITION_DURATION));
            }
        }

        private TestModalUseCase _useCase;
        private TestModalEntity _entity;
        private FakeAsyncPublisher<BaseModalTransitionVO<TestModal>> _publisher;

        [SetUp]
        public void SetUp()
        {
            _entity = new TestModalEntity();
            _publisher = new FakeAsyncPublisher<BaseModalTransitionVO<TestModal>>();
            var subscriber = new FakeAsyncSubscriber<BaseModalTransitionVO<TestModal>>();
            var ports = new TestModalPorts(subscriber, _publisher);
            _useCase = new TestModalUseCase(_entity, ports);
        }

        [TestCase(TestModal.First)]
        [TestCase(TestModal.Second)]
        public async Task FadeAsync_PublishesTransitionOfGivenModalType(TestModal type)
        {
            var modal = new TestModalVO(type, Fade.In);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.type, Is.EqualTo(type));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task FadeAsync_PublishesTransitionWithModalFade(Fade fade)
        {
            var modal = new TestModalVO(TestModal.First, fade);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task FadeAsync_CallsPublisherOnce()
        {
            var modal = new TestModalVO(TestModal.First, Fade.In);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task FadeAsync_SetsPublishedTransitionToEntity()
        {
            var modal = new TestModalVO(TestModal.Second, Fade.Out);

            await _useCase.FadeAsync(modal, CancellationToken.None).AsTask();

            Assert.That(_entity.value, Is.SameAs(_publisher.LastPublished));
        }
    }
}
