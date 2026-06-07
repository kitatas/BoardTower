using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Boot.Application;
using BoardTower.Boot.Data.DataStore;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.Repository;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Boot.Domain.UseCase
{
    [TestFixture]
    public sealed class SplashUseCaseTests
    {
        private SplashUseCase _useCase;
        private FakeAsyncPublisher<SplashTransitionVO> _publisher;
        private SplashTable _splashTable;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<SplashTransitionVO>();
            var subscriber = new FakeAsyncSubscriber<SplashTransitionVO>();
            var ports = new SplashPorts(subscriber, _publisher);

            _splashTable = ScriptableObject.CreateInstance<SplashTable>();
            SetTableList(_splashTable, new List<SplashData>());
            var repository = new SplashRepository(_splashTable);

            _useCase = new SplashUseCase(ports, repository);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
            UnityEngine.Object.DestroyImmediate(_splashTable);
        }

        [Test]
        public void Transition_ReturnsNonNull()
        {
            Assert.That(_useCase.transition, Is.Not.Null);
        }

        [Test]
        public async Task InitAsync_PublishesFadeOutTransition()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
            Assert.That(_publisher.LastPublished.transition.fade, Is.EqualTo(Fade.Out));
            Assert.That(_publisher.LastPublished.transition.duration, Is.EqualTo(0.0f));
        }

        [Test]
        public async Task InitAsync_PublishesNullSplash()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.splash, Is.Null);
        }

        [Test]
        public void NotifyTapScreen_DoesNotThrow()
        {
            Assert.That(() => _useCase.NotifyTapScreen(), Throws.Nothing);
        }

        [Test]
        public void NotifyTapScreen_CalledMultipleTimes_DoesNotThrow()
        {
            _useCase.NotifyTapScreen();
            _useCase.NotifyTapScreen();

            Assert.That(() => _useCase.NotifyTapScreen(), Throws.Nothing);
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }

        [Test]
        public void NotifyTapScreen_AfterDispose_DoesNotThrow()
        {
            ((IDisposable)_useCase).Dispose();

            Assert.That(() => _useCase.NotifyTapScreen(), Throws.Nothing);
        }

        private static void SetTableList(SplashTable table, List<SplashData> items)
        {
            var field = typeof(BaseTable<SplashData>)
                .GetField("list", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(table, items);
        }
    }
}
