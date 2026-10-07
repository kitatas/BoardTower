using BoardTower.Boot.Application;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Boot.Application
{
    [TestFixture]
    public sealed class ConstTests
    {
        [Test]
        public void SplashConfig_Types_DoesNotContainNone()
        {
            // None は SplashVO の生成で QuitExceptionVO になるため、表示対象に含めてはならない
            Assert.That(SplashConfig.TYPES, Has.None.EqualTo(SplashType.None));
        }

        [Test]
        public void SplashConfig_Types_HasNoDuplicates()
        {
            // 重複すると同じスプラッシュが二度表示されてしまう
            Assert.That(SplashConfig.TYPES, Is.Unique);
        }
    }
}
