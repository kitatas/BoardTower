using System;
using BoardTower.Boot.Domain.UseCase;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Domain.UseCase
{
    [TestFixture]
    public sealed class DisplayNameUseCaseTests
    {
        private DisplayNameUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _useCase = new DisplayNameUseCase();
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void HandleDisplayName_DoesNotThrow()
        {
            Assert.That(() => _useCase.HandleDisplayName("TestName"), Throws.Nothing);
        }

        [Test]
        public void HandleDisplayName_WithEmptyString_DoesNotThrow()
        {
            Assert.That(() => _useCase.HandleDisplayName(""), Throws.Nothing);
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }

        [Test]
        public void HandleDisplayName_AfterDispose_DoesNotThrow()
        {
            ((IDisposable)_useCase).Dispose();

            Assert.That(() => _useCase.HandleDisplayName("test"), Throws.Nothing);
        }
    }
}
