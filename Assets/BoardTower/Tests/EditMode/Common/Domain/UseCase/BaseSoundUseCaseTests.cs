using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using BoardTower.Common.Domain.Repository;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using R3;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    /// <summary>
    /// 抽象クラス BaseSoundUseCase&lt;TType, TSoundVO&gt; のテスト。
    /// SaveRepository (ES3 を使用) の実処理を避けるため、保存・ロードを差し替えたテスト用サブクラスで検証する。
    /// </summary>
    [TestFixture]
    public sealed class BaseSoundUseCaseTests
    {
        private sealed class TestSoundUseCase : BaseSoundUseCase<SeType, SeSoundVO>
        {
            public (VolumeVO thisVolume, VolumeVO masterVolume) loadResult;
            public int saveVolumeCount;

            public TestSoundUseCase(SaveRepository saveRepository, SoundRepository soundRepository) : base(
                saveRepository, soundRepository)
            {
            }

            protected override UniTask<(VolumeVO thisVolume, VolumeVO masterVolume)> LoadVolumeAsync(
                CancellationToken token)
            {
                return UniTask.FromResult(loadResult);
            }

            public override void SaveVolume()
            {
                saveVolumeCount++;
            }

            protected override SeSoundVO CreateSound(AudioVO<SeType> audio, float delay)
            {
                return new SeSoundVO(audio, delay, isMute.CurrentValue);
            }
        }

        private TestSoundUseCase _useCase;
        private BgmTable _bgmTable;
        private SeTable _seTable;
        private SeData _decisionData;
        private CompositeDisposable _disposables;

        [SetUp]
        public void SetUp()
        {
            _bgmTable = ScriptableObject.CreateInstance<BgmTable>();
            _seTable = ScriptableObject.CreateInstance<SeTable>();
            _decisionData = SoundTableHelper.CreateSeData(SeType.Decision);
            SoundTableHelper.SetTableList(_bgmTable, new List<BgmData>());
            SoundTableHelper.SetTableList(_seTable, new List<SeData> { _decisionData });

            var soundRepository = new SoundRepository(_bgmTable, _seTable);
            // SaveRepository は本テストでは呼び出さない（コンストラクタは ES3 に触れない）
            _useCase = new TestSoundUseCase(new SaveRepository(), soundRepository);
            _disposables = new CompositeDisposable();
        }

        [TearDown]
        public void TearDown()
        {
            _disposables.Dispose();
            ((IDisposable)_useCase).Dispose();
            UnityEngine.Object.DestroyImmediate(_decisionData);
            UnityEngine.Object.DestroyImmediate(_bgmTable);
            UnityEngine.Object.DestroyImmediate(_seTable);
        }

        [TestCase(-0.5f, 0.0f)]
        [TestCase(0.0f, 0.0f)]
        [TestCase(0.3f, 0.3f)]
        [TestCase(1.0f, 1.0f)]
        [TestCase(1.5f, 1.0f)]
        public void SetVolume_WithValue_ClampsToZeroOneRange(float value, float expected)
        {
            _useCase.SetVolume(value);

            Assert.That(_useCase.thisVolumeValue, Is.EqualTo(expected).Within(1e-6f));
        }

        [TestCase(-0.5f, 0.0f)]
        [TestCase(0.0f, 0.0f)]
        [TestCase(0.3f, 0.3f)]
        [TestCase(1.0f, 1.0f)]
        [TestCase(1.5f, 1.0f)]
        public void SetMasterVolume_WithValue_ClampsToZeroOneRange(float value, float expected)
        {
            _useCase.SetMasterVolume(value);

            Assert.That(_useCase.masterVolumeValue, Is.EqualTo(expected).Within(1e-6f));
        }

        [Test]
        public void SetVolume_DoesNotChangeMasterVolume()
        {
            _useCase.SetMasterVolume(0.4f);

            _useCase.SetVolume(0.9f);

            Assert.That(_useCase.masterVolumeValue, Is.EqualTo(0.4f).Within(1e-6f));
        }

        [Test]
        public void Volume_IsProductOfThisAndMasterVolume()
        {
            _useCase.SetVolume(0.5f);
            _useCase.SetMasterVolume(0.4f);

            Assert.That(_useCase.volume.CurrentValue, Is.EqualTo(0.2f).Within(1e-6f));
        }

        [Test]
        public void Volume_WhenMasterVolumeIsZero_IsZero()
        {
            _useCase.SetVolume(1.0f);
            _useCase.SetMasterVolume(0.0f);

            Assert.That(_useCase.volume.CurrentValue, Is.EqualTo(0.0f).Within(1e-6f));
        }

        [Test]
        public async Task LoadAsync_SetsThisVolumeFromLoadedData()
        {
            _useCase.loadResult = (new VolumeVO(0.8f, false), new VolumeVO(0.5f, false));

            await _useCase.LoadAsync(CancellationToken.None).AsTask();

            Assert.That(_useCase.thisVolumeValue, Is.EqualTo(0.8f).Within(1e-6f));
        }

        [Test]
        public async Task LoadAsync_SetsMasterVolumeFromLoadedData()
        {
            _useCase.loadResult = (new VolumeVO(0.8f, false), new VolumeVO(0.5f, false));

            await _useCase.LoadAsync(CancellationToken.None).AsTask();

            Assert.That(_useCase.masterVolumeValue, Is.EqualTo(0.5f).Within(1e-6f));
        }

        [Test]
        public async Task LoadAsync_WithOutOfRangeVolume_ClampsVolume()
        {
            _useCase.loadResult = (new VolumeVO(1.5f, false), new VolumeVO(-1.0f, false));

            await _useCase.LoadAsync(CancellationToken.None).AsTask();

            Assert.That(_useCase.thisVolumeValue, Is.EqualTo(1.0f).Within(1e-6f));
        }

        [Test]
        public async Task LoadAsync_SetsThisMuteFromThisVolumeData()
        {
            _useCase.loadResult = (new VolumeVO(0.5f, true), new VolumeVO(0.5f, false));

            await _useCase.LoadAsync(CancellationToken.None).AsTask();

            Assert.That(_useCase.isThisMute.CurrentValue, Is.True);
        }

        [Test]
        public async Task LoadAsync_SetsMasterMuteFromMasterVolumeData()
        {
            _useCase.loadResult = (new VolumeVO(0.5f, false), new VolumeVO(0.5f, true));

            await _useCase.LoadAsync(CancellationToken.None).AsTask();

            Assert.That(_useCase.isMasterMute.CurrentValue, Is.True);
        }

        [Test]
        public async Task LoadAsync_WhenOnlyMasterIsMuted_DoesNotMuteThis()
        {
            _useCase.loadResult = (new VolumeVO(0.5f, false), new VolumeVO(0.5f, true));

            await _useCase.LoadAsync(CancellationToken.None).AsTask();

            Assert.That(_useCase.isThisMute.CurrentValue, Is.False);
        }

        [Test]
        public void SwitchMute_WhenUnmuted_MutesThis()
        {
            _useCase.SwitchMute();

            Assert.That(_useCase.isThisMute.CurrentValue, Is.True);
        }

        [Test]
        public void SwitchMute_CalledTwice_UnmutesThis()
        {
            _useCase.SwitchMute();
            _useCase.SwitchMute();

            Assert.That(_useCase.isThisMute.CurrentValue, Is.False);
        }

        [Test]
        public void SwitchMute_DoesNotChangeMasterMute()
        {
            _useCase.SwitchMute();

            Assert.That(_useCase.isMasterMute.CurrentValue, Is.False);
        }

        [Test]
        public void SwitchMute_SavesVolume()
        {
            _useCase.SwitchMute();

            Assert.That(_useCase.saveVolumeCount, Is.EqualTo(1));
        }

        [Test]
        public void SwitchMasterMute_WhenUnmuted_MutesMaster()
        {
            _useCase.SwitchMasterMute();

            Assert.That(_useCase.isMasterMute.CurrentValue, Is.True);
        }

        [Test]
        public void SwitchMasterMute_CalledTwice_UnmutesMaster()
        {
            _useCase.SwitchMasterMute();
            _useCase.SwitchMasterMute();

            Assert.That(_useCase.isMasterMute.CurrentValue, Is.False);
        }

        [Test]
        public void SwitchMasterMute_DoesNotChangeThisMute()
        {
            _useCase.SwitchMasterMute();

            Assert.That(_useCase.isThisMute.CurrentValue, Is.False);
        }

        [TestCase(false, false, false)]
        [TestCase(true, false, true)]
        [TestCase(false, true, true)]
        [TestCase(true, true, true)]
        public void IsMute_IsLogicalOrOfThisAndMasterMute(bool thisMute, bool masterMute, bool expected)
        {
            if (thisMute) _useCase.SwitchMute();
            if (masterMute) _useCase.SwitchMasterMute();

            Assert.That(_useCase.isMute.CurrentValue, Is.EqualTo(expected));
        }

        [Test]
        public void Play_WithRegisteredType_EmitsSoundWithRepositoryAudio()
        {
            SeSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(SeType.Decision);

            Assert.That(played.audio.type, Is.EqualTo(SeType.Decision));
        }

        [Test]
        public void Play_WithDelay_EmitsSoundWithDelay()
        {
            SeSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(SeType.Decision, 0.75f);

            Assert.That(played.delay, Is.EqualTo(0.75f).Within(1e-6f));
        }

        [Test]
        public void Play_WithoutDelay_EmitsSoundWithZeroDelay()
        {
            SeSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(SeType.Decision);

            Assert.That(played.delay, Is.EqualTo(0.0f).Within(1e-6f));
        }

        [Test]
        public void Play_WhenMuted_EmitsMutedSound()
        {
            _useCase.SwitchMasterMute();
            SeSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(SeType.Decision);

            Assert.That(played.isMute, Is.True);
        }

        [Test]
        public void Play_WhenNotMuted_EmitsUnmutedSound()
        {
            SeSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(SeType.Decision);

            Assert.That(played.isMute, Is.False);
        }

        [Test]
        public void Play_WithUnregisteredType_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _useCase.Play(SeType.Cancel), Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void Play_WithUnregisteredType_DoesNotEmitSound()
        {
            var count = 0;
            _disposables.Add(_useCase.play.Subscribe(_ => count++));

            Assert.Throws<QuitExceptionVO>(() => _useCase.Play(SeType.Cancel));

            Assert.That(count, Is.EqualTo(0));
        }
    }
}
