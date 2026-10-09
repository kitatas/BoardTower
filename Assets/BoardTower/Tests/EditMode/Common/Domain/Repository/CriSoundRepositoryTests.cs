using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Domain.Repository
{
    [TestFixture]
    public sealed class CriSoundRepositoryTests
    {
        private CriSoundRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new CriSoundRepository();
        }

        // ---- Find(BgmType) ----

        [Test]
        public void FindBgm_WithNone_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _repository.Find(BgmType.None), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void FindBgm_WithNone_ThrowsNotFoundBgmMessage()
        {
            Assert.That(() => _repository.Find(BgmType.None),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.NOT_FOUND_BGM));
        }

        [TestCase(BgmType.Top, "Top")]
        [TestCase(BgmType.Game, "Game")]
        public void FindBgm_WithValidType_ReturnsAudioWithTypeAndCue(BgmType type, string expectedCue)
        {
            var result = _repository.Find(type);

            Assert.That(result.type, Is.EqualTo(type));
            Assert.That(result.cue, Is.EqualTo(expectedCue));
        }

        [Test]
        public void FindBgm_WithValidType_DoesNotSetClip()
        {
            var result = _repository.Find(BgmType.Top);

            Assert.That(result.clip == null, Is.True);
        }

        // ---- Find(SeType) ----

        [Test]
        public void FindSe_WithNone_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _repository.Find(SeType.None), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void FindSe_WithNone_ThrowsNotFoundSeMessage()
        {
            Assert.That(() => _repository.Find(SeType.None),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.NOT_FOUND_SE));
        }

        [TestCase(SeType.Decision, "Decision")]
        [TestCase(SeType.Cancel, "Cancel")]
        public void FindSe_WithValidType_ReturnsAudioWithTypeAndCue(SeType type, string expectedCue)
        {
            var result = _repository.Find(type);

            Assert.That(result.type, Is.EqualTo(type));
            Assert.That(result.cue, Is.EqualTo(expectedCue));
        }

        [Test]
        public void FindSe_WithValidType_DoesNotSetClip()
        {
            var result = _repository.Find(SeType.Decision);

            Assert.That(result.clip == null, Is.True);
        }

        // ---- Find<T>(T) ----

        [TestCase(BgmType.Top)]
        [TestCase(BgmType.Game)]
        public void FindGeneric_WithBgmType_ReturnsBgmAudioVOWithCue(BgmType type)
        {
            var result = _repository.Find<BgmType>(type);

            Assert.That(result, Is.TypeOf<BgmAudioVO>());
            Assert.That(result.type, Is.EqualTo(type));
            Assert.That(result.cue, Is.EqualTo(type.ToString()));
        }

        [TestCase(SeType.Decision)]
        [TestCase(SeType.Cancel)]
        public void FindGeneric_WithSeType_ReturnsSeAudioVOWithCue(SeType type)
        {
            var result = _repository.Find<SeType>(type);

            Assert.That(result, Is.TypeOf<SeAudioVO>());
            Assert.That(result.type, Is.EqualTo(type));
            Assert.That(result.cue, Is.EqualTo(type.ToString()));
        }

        [Test]
        public void FindGeneric_WithBgmNone_ThrowsNotFoundBgmMessage()
        {
            Assert.That(() => _repository.Find<BgmType>(BgmType.None),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.NOT_FOUND_BGM));
        }

        [Test]
        public void FindGeneric_WithSeNone_ThrowsNotFoundSeMessage()
        {
            Assert.That(() => _repository.Find<SeType>(SeType.None),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.NOT_FOUND_SE));
        }

        [Test]
        public void FindGeneric_WithUnsupportedEnum_ThrowsNotFoundSoundTypeMessage()
        {
            Assert.That(() => _repository.Find<Fade>(Fade.In),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.NOT_FOUND_SOUND_TYPE));
        }
    }
}
