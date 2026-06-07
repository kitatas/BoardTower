using BoardTower.Boot.Application;
using BoardTower.Common.Application;
using NUnit.Framework;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Boot.Application
{
    [TestFixture]
    public sealed class ValueObjectTests
    {
        // ---- DisplayNameTransitionVO ----

        [Test]
        public void DisplayNameTransitionVO_Constructor_AssignsTransition()
        {
            var transition = new TransitionVO(Fade.In, 0.5f);
            var sut = new DisplayNameTransitionVO(transition);
            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [TestCase(Fade.In, 0.0f)]
        [TestCase(Fade.Out, 0.5f)]
        [TestCase(Fade.InOut, 1.0f)]
        public void DisplayNameTransitionVO_Create_SetsCorrectFadeAndDuration(Fade fade, float duration)
        {
            var sut = DisplayNameTransitionVO.Create(fade, duration);
            Assert.That(sut.transition.fade, Is.EqualTo(fade));
            Assert.That(sut.transition.duration, Is.EqualTo(duration));
        }

        // ---- SplashVO ----

        [Test]
        public void SplashVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            var sprite = CreateSprite();
            Assert.That(() => new SplashVO(SplashType.None, sprite), Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(SplashType.Developer)]
        [TestCase(SplashType.PlayFab)]
        public void SplashVO_Constructor_WithValidType_SetsType(SplashType splashType)
        {
            var sprite = CreateSprite();
            var sut = new SplashVO(splashType, sprite);
            Assert.That(sut.type, Is.EqualTo(splashType));
        }

        [Test]
        public void SplashVO_Constructor_AssignsSprite()
        {
            var sprite = CreateSprite();
            var sut = new SplashVO(SplashType.Developer, sprite);
            Assert.That(sut.sprite, Is.EqualTo(sprite));
        }

        // ---- SplashTransitionVO ----

        [Test]
        public void SplashTransitionVO_Constructor_AssignsSplashAndTransition()
        {
            var splash = new SplashVO(SplashType.Developer, CreateSprite());
            var transition = new TransitionVO(Fade.In, 0.5f);
            var sut = new SplashTransitionVO(splash, transition);
            Assert.That(sut.splash, Is.EqualTo(splash));
            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [TestCase(Fade.In, 0.0f)]
        [TestCase(Fade.Out, 0.5f)]
        [TestCase(Fade.InOut, 1.0f)]
        public void SplashTransitionVO_Create_SetsCorrectFadeAndDuration(Fade fade, float duration)
        {
            var splash = new SplashVO(SplashType.Developer, CreateSprite());
            var sut = SplashTransitionVO.Create(splash, fade, duration);
            Assert.That(sut.splash, Is.EqualTo(splash));
            Assert.That(sut.transition.fade, Is.EqualTo(fade));
            Assert.That(sut.transition.duration, Is.EqualTo(duration));
        }

        private static Sprite CreateSprite()
        {
            var texture = new Texture2D(1, 1);
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
        }
    }
}
