using System.Collections.Generic;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class SceneUseCaseTests
    {
        private SceneUseCase _useCase;
        private CompositeDisposable _disposables;

        [SetUp]
        public void SetUp()
        {
            _useCase = new SceneUseCase();
            _disposables = new CompositeDisposable();
        }

        [TearDown]
        public void TearDown()
        {
            _disposables.Dispose();
            _useCase.Dispose();
        }

        [TestCase(SceneName.Boot)]
        [TestCase(SceneName.Game)]
        public void Load_WithSceneName_NotifiesLoadWithSceneName(SceneName sceneName)
        {
            LoadVO received = null;
            _disposables.Add(_useCase.load.Subscribe(x => received = x));

            _useCase.Load(sceneName, LoadType.Direct);

            Assert.That(received.sceneName, Is.EqualTo(sceneName));
        }

        [TestCase(LoadType.Direct)]
        [TestCase(LoadType.Fade)]
        public void Load_WithLoadType_NotifiesLoadWithLoadType(LoadType loadType)
        {
            LoadVO received = null;
            _disposables.Add(_useCase.load.Subscribe(x => received = x));

            _useCase.Load(SceneName.Game, loadType);

            Assert.That(received.loadType, Is.EqualTo(loadType));
        }

        [Test]
        public void Load_CalledTwice_NotifiesEachTime()
        {
            var received = new List<LoadVO>();
            _disposables.Add(_useCase.load.Subscribe(x => received.Add(x)));

            _useCase.Load(SceneName.Boot, LoadType.Direct);
            _useCase.Load(SceneName.Game, LoadType.Fade);

            Assert.That(received.Count, Is.EqualTo(2));
        }

        [Test]
        public void Load_WithoutSubscriber_DoesNotThrow()
        {
            Assert.That(() => _useCase.Load(SceneName.Boot, LoadType.Direct), Throws.Nothing);
        }

        [Test]
        public void Load_WithSceneNameNone_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _useCase.Load(SceneName.None, LoadType.Direct), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void Load_WithLoadTypeNone_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _useCase.Load(SceneName.Boot, LoadType.None), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void Load_WithInvalidArguments_DoesNotNotify()
        {
            var count = 0;
            _disposables.Add(_useCase.load.Subscribe(_ => count++));

            Assert.Throws<QuitExceptionVO>(() => _useCase.Load(SceneName.None, LoadType.Direct));

            Assert.That(count, Is.EqualTo(0));
        }
    }
}
