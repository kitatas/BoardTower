using System;
using System.Collections.Generic;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using BoardTower.Common.Domain.Repository;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using R3;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Common.Domain.UseCase
{
    /// <summary>
    /// BgmUseCase のテスト。
    /// SaveVolume / LoadAsync / SwitchMute / SwitchMasterMute は ES3 (ファイルI/O) に依存するため対象外。
    /// 共通ロジックは BaseSoundUseCaseTests で検証済みのため、BgmUseCase 固有の振る舞い (BgmSoundVO の生成) に絞る。
    /// </summary>
    [TestFixture]
    public sealed class BgmUseCaseTests
    {
        private BgmUseCase _useCase;
        private SoundRepository _soundRepository;
        private BgmTable _bgmTable;
        private SeTable _seTable;
        private BgmData _topData;
        private CompositeDisposable _disposables;

        [SetUp]
        public void SetUp()
        {
            _bgmTable = ScriptableObject.CreateInstance<BgmTable>();
            _seTable = ScriptableObject.CreateInstance<SeTable>();
            _topData = SoundTableHelper.CreateBgmData(BgmType.Top);
            SoundTableHelper.SetTableList(_bgmTable, new List<BgmData> { _topData });
            SoundTableHelper.SetTableList(_seTable, new List<SeData>());

            _soundRepository = new SoundRepository(_bgmTable, _seTable);
            // SaveRepository は本テストでは呼び出さない（コンストラクタは ES3 に触れない）
            _useCase = new BgmUseCase(new SaveRepository(), _soundRepository);
            _disposables = new CompositeDisposable();
        }

        [TearDown]
        public void TearDown()
        {
            _disposables.Dispose();
            ((IDisposable)_useCase).Dispose();
            UnityEngine.Object.DestroyImmediate(_topData);
            UnityEngine.Object.DestroyImmediate(_bgmTable);
            UnityEngine.Object.DestroyImmediate(_seTable);
        }

        [Test]
        public void Play_EmitsSoundWithRepositoryAudio()
        {
            BgmSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(BgmType.Top);

            Assert.That(played.audio, Is.SameAs(_soundRepository.Find(BgmType.Top)));
        }

        [Test]
        public void Play_WithDelay_EmitsSoundWithDelay()
        {
            BgmSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(BgmType.Top, 0.5f);

            Assert.That(played.delay, Is.EqualTo(0.5f).Within(1e-6f));
        }

        [Test]
        public void Play_WhenNotMuted_EmitsUnmutedSound()
        {
            BgmSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(BgmType.Top);

            Assert.That(played.isMute, Is.False);
        }

        [Test]
        public void Play_WithUnregisteredType_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _useCase.Play(BgmType.Game), Throws.TypeOf<QuitExceptionVO>());
        }
    }
}
