using BoardTower.Common.Application;
using NUnit.Framework;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class ValueObjectTests
    {
        // ---- LoadVO ----

        [Test]
        public void LoadVO_Constructor_WithNoneSceneName_ThrowsQuitExceptionVO()
        {
            Assert.That(() => new LoadVO(SceneName.None, LoadType.Direct), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void LoadVO_Constructor_WithNoneLoadType_ThrowsQuitExceptionVO()
        {
            Assert.That(() => new LoadVO(SceneName.Boot, LoadType.None), Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(SceneName.Boot, LoadType.Direct)]
        [TestCase(SceneName.Boot, LoadType.Fade)]
        [TestCase(SceneName.Game, LoadType.Direct)]
        [TestCase(SceneName.Game, LoadType.Fade)]
        public void LoadVO_Constructor_WithValidParams_AssignsValues(SceneName sceneName, LoadType loadType)
        {
            var sut = new LoadVO(sceneName, loadType);

            Assert.That(sut.sceneName, Is.EqualTo(sceneName));
            Assert.That(sut.loadType, Is.EqualTo(loadType));
        }

        // ---- RebootExceptionVO ----

        [Test]
        public void RebootExceptionVO_Constructor_AssignsMessage()
        {
            var sut = new RebootExceptionVO("test");

            Assert.That(sut.Message, Is.EqualTo("test"));
        }

        [Test]
        public void RebootExceptionVO_ExceptionMessage_ReturnsRebootMessage()
        {
            var sut = new RebootExceptionVO("test");

            Assert.That(sut.exceptionMessage, Is.EqualTo(ExceptionConfig.REBOOT_MESSAGE));
        }

        // ---- RetryExceptionVO ----

        [Test]
        public void RetryExceptionVO_Constructor_AssignsMessage()
        {
            var sut = new RetryExceptionVO("test");

            Assert.That(sut.Message, Is.EqualTo("test"));
        }

        [Test]
        public void RetryExceptionVO_ExceptionMessage_ReturnsRetryMessage()
        {
            var sut = new RetryExceptionVO("test");

            Assert.That(sut.exceptionMessage, Is.EqualTo(ExceptionConfig.RETRY_MESSAGE));
        }

        // ---- QuitExceptionVO ----

        [Test]
        public void QuitExceptionVO_Constructor_AssignsMessage()
        {
            var sut = new QuitExceptionVO("test");

            Assert.That(sut.Message, Is.EqualTo("test"));
        }

        [Test]
        public void QuitExceptionVO_ExceptionMessage_ReturnsQuitMessage()
        {
            var sut = new QuitExceptionVO("test");

            Assert.That(sut.exceptionMessage, Is.EqualTo(ExceptionConfig.QUIT_MESSAGE));
        }

        // ---- TransitionVO ----

        [Test]
        public void TransitionVO_Constructor_WithNoneFade_ThrowsQuitExceptionVO()
        {
            Assert.That(() => new TransitionVO(Fade.None), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void TransitionVO_Constructor_WithNegativeDuration_ThrowsQuitExceptionVO()
        {
            Assert.That(() => new TransitionVO(Fade.In, -0.1f), Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(Fade.In, 0.0f)]
        [TestCase(Fade.Out, 0.5f)]
        [TestCase(Fade.InOut, 1.0f)]
        public void TransitionVO_Constructor_WithValidParams_AssignsValues(Fade fade, float duration)
        {
            var sut = new TransitionVO(fade, duration);

            Assert.That(sut.fade, Is.EqualTo(fade));
            Assert.That(sut.duration, Is.EqualTo(duration));
        }

        [Test]
        public void TransitionVO_Constructor_WithZeroDuration_IsValid()
        {
            var sut = new TransitionVO(Fade.In, 0.0f);

            Assert.That(sut.duration, Is.EqualTo(0.0f));
        }

        // ---- ExceptionNotifyVO ----

        [Test]
        public void ExceptionNotifyVO_Constructor_AssignsExceptionAndTransition()
        {
            var exception = new QuitExceptionVO("msg");
            var transition = new TransitionVO(Fade.In, 0.5f);

            var sut = new ExceptionNotifyVO(exception, transition);

            Assert.That(sut.exception, Is.EqualTo(exception));
            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [Test]
        public void ExceptionNotifyVO_Constructor_WithNullException_AssignsNull()
        {
            var transition = new TransitionVO(Fade.Out, 0.0f);

            var sut = new ExceptionNotifyVO(null, transition);

            Assert.That(sut.exception, Is.Null);
        }

        [TestCase(Fade.In, 0.25f)]
        [TestCase(Fade.Out, 0.0f)]
        [TestCase(Fade.InOut, 1.0f)]
        public void ExceptionNotifyVO_Create_SetsCorrectFadeAndDuration(Fade fade, float duration)
        {
            var exception = new QuitExceptionVO("msg");

            var sut = ExceptionNotifyVO.Create(exception, fade, duration);

            Assert.That(sut.exception, Is.EqualTo(exception));
            Assert.That(sut.transition.fade, Is.EqualTo(fade));
            Assert.That(sut.transition.duration, Is.EqualTo(duration));
        }

        // ---- ExceptionActionVO ----

        [Test]
        public void ExceptionActionVO_Constructor_AssignsException()
        {
            var exception = new RetryExceptionVO("retry");

            var sut = new ExceptionActionVO(exception);

            Assert.That(sut.exception, Is.EqualTo(exception));
        }

        // ---- LoadingTransitionVO ----

        [Test]
        public void LoadingTransitionVO_Constructor_AssignsTransition()
        {
            var transition = new TransitionVO(Fade.In, 0.5f);

            var sut = new LoadingTransitionVO(transition);

            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [TestCase(Fade.In, 0.0f)]
        [TestCase(Fade.Out, 0.5f)]
        [TestCase(Fade.InOut, 1.0f)]
        public void LoadingTransitionVO_Create_SetsCorrectFadeAndDuration(Fade fade, float duration)
        {
            var sut = LoadingTransitionVO.Create(fade, duration);

            Assert.That(sut.transition.fade, Is.EqualTo(fade));
            Assert.That(sut.transition.duration, Is.EqualTo(duration));
        }

        // ---- BgmAudioVO ----

        [Test]
        public void BgmAudioVO_Constructor_AssignsTypeAndClip()
        {
            var sut = new BgmAudioVO(BgmType.Top, null);

            Assert.That(sut.type, Is.EqualTo(BgmType.Top));
            Assert.That(sut.clip, Is.Null);
        }

        [TestCase(BgmType.None)]
        [TestCase(BgmType.Top)]
        [TestCase(BgmType.Game)]
        public void BgmAudioVO_Constructor_WithAllTypes_SetsType(BgmType type)
        {
            var sut = new BgmAudioVO(type, null);

            Assert.That(sut.type, Is.EqualTo(type));
        }

        // ---- SeAudioVO ----

        [Test]
        public void SeAudioVO_Constructor_AssignsTypeAndClip()
        {
            var sut = new SeAudioVO(SeType.Decision, null);

            Assert.That(sut.type, Is.EqualTo(SeType.Decision));
            Assert.That(sut.clip, Is.Null);
        }

        [TestCase(SeType.None)]
        [TestCase(SeType.Decision)]
        [TestCase(SeType.Cancel)]
        public void SeAudioVO_Constructor_WithAllTypes_SetsType(SeType type)
        {
            var sut = new SeAudioVO(type, null);

            Assert.That(sut.type, Is.EqualTo(type));
        }

        // ---- BgmSoundVO ----

        [Test]
        public void BgmSoundVO_Constructor_AssignsAudioDelayAndMute()
        {
            var audio = new BgmAudioVO(BgmType.Top, null);

            var sut = new BgmSoundVO(audio, 0.5f, true);

            Assert.That(sut.audio, Is.EqualTo(audio));
            Assert.That(sut.delay, Is.EqualTo(0.5f));
            Assert.That(sut.isMute, Is.True);
        }

        // ---- SeSoundVO ----

        [Test]
        public void SeSoundVO_Constructor_AssignsAudioDelayAndMute()
        {
            var audio = new SeAudioVO(SeType.Decision, null);

            var sut = new SeSoundVO(audio, 0.0f, false);

            Assert.That(sut.audio, Is.EqualTo(audio));
            Assert.That(sut.delay, Is.EqualTo(0.0f));
            Assert.That(sut.isMute, Is.False);
        }

        // ---- LocalUserVO ----

        [Test]
        public void LocalUserVO_Constructor_AssignsId()
        {
            var sut = new LocalUserVO("user-123");

            Assert.That(sut.id, Is.EqualTo("user-123"));
        }

        [Test]
        public void LocalUserVO_Constructor_WithEmptyId_AssignsEmptyString()
        {
            var sut = new LocalUserVO("");

            Assert.That(sut.id, Is.EqualTo(""));
        }

        // ---- VolumeVO ----

        [Test]
        public void VolumeVO_Constructor_AssignsValueAndMute()
        {
            var sut = new VolumeVO(0.7f, false);

            Assert.That(sut.value, Is.EqualTo(0.7f));
            Assert.That(sut.isMute, Is.False);
        }

        [TestCase(0.0f, false)]
        [TestCase(0.5f, true)]
        [TestCase(1.0f, false)]
        public void VolumeVO_Constructor_WithVariousValues_AssignsCorrectly(float value, bool isMute)
        {
            var sut = new VolumeVO(value, isMute);

            Assert.That(sut.value, Is.EqualTo(value));
            Assert.That(sut.isMute, Is.EqualTo(isMute));
        }

        // ---- UserDisplayNameVO ----

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        public void UserDisplayNameVO_Constructor_WithEmptyOrWhitespace_ThrowsRetryExceptionVO(string value)
        {
            Assert.That(() => new UserDisplayNameVO(value), Throws.TypeOf<RetryExceptionVO>());
        }

        [TestCase("ab")]
        [TestCase("a")]
        public void UserDisplayNameVO_Constructor_WithTooShortValue_ThrowsRetryExceptionVO(string value)
        {
            Assert.That(() => new UserDisplayNameVO(value), Throws.TypeOf<RetryExceptionVO>());
        }

        [TestCase("abcdefghijk")]
        [TestCase("123456789012")]
        public void UserDisplayNameVO_Constructor_WithTooLongValue_ThrowsRetryExceptionVO(string value)
        {
            Assert.That(() => new UserDisplayNameVO(value), Throws.TypeOf<RetryExceptionVO>());
        }

        [TestCase("abc")]
        [TestCase("abcde")]
        [TestCase("abcdefghij")]
        public void UserDisplayNameVO_Constructor_WithValidValue_AssignsValue(string value)
        {
            var sut = new UserDisplayNameVO(value);

            Assert.That(sut.value, Is.EqualTo(value));
        }

        [Test]
        public void UserDisplayNameVO_Create_ReturnsInstanceWithEmptyValue()
        {
            var sut = UserDisplayNameVO.Create();

            Assert.That(sut.value, Is.EqualTo(""));
        }

        // ---- PlayFabUserVO ----

        [Test]
        public void PlayFabUserVO_Constructor_AssignsIsNewlyAndDisplayName()
        {
            var displayName = new UserDisplayNameVO("TestUser");

            var sut = new PlayFabUserVO(true, displayName);

            Assert.That(sut.isNewly, Is.True);
            Assert.That(sut.displayName, Is.EqualTo(displayName));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PlayFabUserVO_Constructor_WithIsNewlyVariants_SetsCorrectly(bool isNewly)
        {
            var displayName = UserDisplayNameVO.Create();

            var sut = new PlayFabUserVO(isNewly, displayName);

            Assert.That(sut.isNewly, Is.EqualTo(isNewly));
        }

        // ---- UserVO ----

        [Test]
        public void UserVO_Constructor_AssignsLocalUserAndPlayFabUser()
        {
            var localUser = new LocalUserVO("id-001");
            var playFabUser = new PlayFabUserVO(false, UserDisplayNameVO.Create());

            var sut = new UserVO(localUser, playFabUser);

            Assert.That(sut.localUser, Is.EqualTo(localUser));
            Assert.That(sut.playFabUser, Is.EqualTo(playFabUser));
        }

        // ---- SaveVO ----

        [Test]
        public void SaveVO_Constructor_AssignsAllFields()
        {
            var user = new LocalUserVO("id-001");
            var master = new VolumeVO(0.7f, false);
            var bgm = new VolumeVO(0.5f, false);
            var se = new VolumeVO(0.3f, true);

            var sut = new SaveVO(user, master, bgm, se);

            Assert.That(sut.user, Is.EqualTo(user));
            Assert.That(sut.masterVolume, Is.EqualTo(master));
            Assert.That(sut.bgmVolume, Is.EqualTo(bgm));
            Assert.That(sut.seVolume, Is.EqualTo(se));
        }

        // ---- LoginResultVO ----

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void LoginResultVO_Constructor_AssignsIsSuccessAndIsRegistered(bool isSuccess, bool isRegistered)
        {
            var sut = new LoginResultVO(isSuccess, isRegistered);

            Assert.That(sut.isSuccess, Is.EqualTo(isSuccess));
            Assert.That(sut.isRegistered, Is.EqualTo(isRegistered));
        }
    }
}
