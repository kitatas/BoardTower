using BoardTower.Boot.Application;
using BoardTower.Common.Application;
using NUnit.Framework;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Boot.Application
{
    [TestFixture]
    public sealed class ValueObjectTests
    {
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

        // ---- BootModalVO ----

        [Test]
        public void BootModalVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            Assert.That(() => new BootModalVO(BootModalType.None, Fade.In), Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(BootModalType.Update)]
        [TestCase(BootModalType.Name)]
        public void BootModalVO_Constructor_WithValidType_SetsType(BootModalType type)
        {
            var sut = new BootModalVO(type, Fade.In);

            Assert.That(sut.type, Is.EqualTo(type));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public void BootModalVO_Constructor_WithValidFade_SetsFade(Fade fade)
        {
            var sut = new BootModalVO(BootModalType.Update, fade);

            Assert.That(sut.fade, Is.EqualTo(fade));
        }

        // ---- BootModalTransitionVO ----

        [Test]
        public void BootModalTransitionVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            var transition = new TransitionVO(Fade.In, 0.5f);

            Assert.That(() => new BootModalTransitionVO(BootModalType.None, transition),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void BootModalTransitionVO_Create_WithNoneType_ThrowsQuitExceptionVO()
        {
            Assert.That(() => BootModalTransitionVO.Create(BootModalType.None, Fade.In, 0.5f),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(BootModalType.Update)]
        [TestCase(BootModalType.Name)]
        public void BootModalTransitionVO_Create_WithTypeFadeDuration_SetsType(BootModalType type)
        {
            var sut = BootModalTransitionVO.Create(type, Fade.In, 0.5f);

            Assert.That(sut.type, Is.EqualTo(type));
        }

        [TestCase(Fade.In, 0.0f)]
        [TestCase(Fade.Out, 0.5f)]
        [TestCase(Fade.InOut, 1.0f)]
        public void BootModalTransitionVO_Create_WithTypeFadeDuration_SetsFadeAndDuration(Fade fade, float duration)
        {
            var sut = BootModalTransitionVO.Create(BootModalType.Update, fade, duration);

            Assert.That(sut.transition.fade, Is.EqualTo(fade));
            Assert.That(sut.transition.duration, Is.EqualTo(duration));
        }

        [TestCase(BootModalType.Update)]
        [TestCase(BootModalType.Name)]
        public void BootModalTransitionVO_Create_FromBootModal_InheritsType(BootModalType type)
        {
            var modal = new BootModalVO(type, Fade.Out);

            var sut = BootModalTransitionVO.Create(modal, 0.25f);

            Assert.That(sut.type, Is.EqualTo(type));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public void BootModalTransitionVO_Create_FromBootModal_InheritsFade(Fade fade)
        {
            var modal = new BootModalVO(BootModalType.Update, fade);

            var sut = BootModalTransitionVO.Create(modal, 0.25f);

            Assert.That(sut.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public void BootModalTransitionVO_Create_FromBootModal_SetsDuration()
        {
            var modal = new BootModalVO(BootModalType.Update, Fade.In);

            var sut = BootModalTransitionVO.Create(modal, 0.25f);

            Assert.That(sut.transition.duration, Is.EqualTo(0.25f));
        }

        private static Sprite CreateSprite()
        {
            var texture = new Texture2D(1, 1);
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
        }
    }
}
