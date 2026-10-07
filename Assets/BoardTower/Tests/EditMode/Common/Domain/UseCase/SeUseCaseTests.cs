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
    /// SeUseCase のテスト。
    /// SaveVolume / LoadAsync は ES3 (ファイルI/O) に依存するため対象外。
    /// 共通ロジックは BaseSoundUseCaseTests で検証済みのため、SeUseCase 固有の振る舞い (SeSoundVO の生成) に絞る。
    /// </summary>
    [TestFixture]
    public sealed class SeUseCaseTests
    {
        private SeUseCase _useCase;
        private SoundRepository _soundRepository;
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

            _soundRepository = new SoundRepository(_bgmTable, _seTable);
            // SaveRepository は本テストでは呼び出さない（コンストラクタは ES3 に触れない）
            _useCase = new SeUseCase(new SaveRepository(), _soundRepository);
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

        [Test]
        public void Play_EmitsSoundWithRepositoryAudio()
        {
            SeSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(SeType.Decision);

            Assert.That(played.audio, Is.SameAs(_soundRepository.Find(SeType.Decision)));
        }

        [Test]
        public void Play_WithDelay_EmitsSoundWithDelay()
        {
            SeSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(SeType.Decision, 0.5f);

            Assert.That(played.delay, Is.EqualTo(0.5f).Within(1e-6f));
        }

        [Test]
        public void Play_WhenMasterMuted_EmitsMutedSound()
        {
            // NOTE: SeUseCase の SwitchMasterMute は保存処理を行わない（BgmUseCase とは異なる）
            _useCase.SwitchMasterMute();
            SeSoundVO played = null;
            _disposables.Add(_useCase.play.Subscribe(x => played = x));

            _useCase.Play(SeType.Decision);

            Assert.That(played.isMute, Is.True);
        }

        [Test]
        public void Play_WithUnregisteredType_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _useCase.Play(SeType.Cancel), Throws.TypeOf<QuitExceptionVO>());
        }
    }
}
