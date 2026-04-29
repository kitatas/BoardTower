using System;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class VolumePresenter : IInitializable, IDisposable
    {
        private readonly BgmUseCase _bgmUseCase;
        private readonly SeUseCase _seUseCase;
        private readonly VolumeFacade _volumeFacade;
        private readonly CompositeDisposable _disposable;

        public VolumePresenter(BgmUseCase bgmUseCase, SeUseCase seUseCase, VolumeFacade volumeFacade)
        {
            _bgmUseCase = bgmUseCase;
            _seUseCase = seUseCase;
            _volumeFacade = volumeFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _volumeFacade.Init(_bgmUseCase.volume.CurrentValue, _seUseCase.volume.CurrentValue);

            _volumeFacade.masterVolume
                .Subscribe(x =>
                {
                    _bgmUseCase.SetMasterVolume(x);
                    _seUseCase.SetMasterVolume(x);
                })
                .AddTo(_disposable);

            _volumeFacade.bgmVolume
                .Subscribe(_bgmUseCase.SetVolume)
                .AddTo(_disposable);

            _volumeFacade.seVolume
                .Subscribe(_seUseCase.SetVolume)
                .AddTo(_disposable);

            _volumeFacade.releaseMaster
                // NOTE: 保存の重複を避けるため、bgm側のみ実行
                .Subscribe(_ => _bgmUseCase.SaveMasterVolume())
                .AddTo(_disposable);

            _volumeFacade.releaseBgm
                .Subscribe(_ => _bgmUseCase.SaveVolume())
                .AddTo(_disposable);

            _volumeFacade.releaseSe
                .Subscribe(_ => _seUseCase.SaveVolume())
                .AddTo(_disposable);

            _volumeFacade.releaseAny
                .Subscribe(_ => _seUseCase.Play(SeType.Decision))
                .AddTo(_disposable);

            _volumeFacade.muteMaster
                .Subscribe(_ =>
                {
                    _bgmUseCase.SwitchMasterMute();
                    _seUseCase.SwitchMasterMute();
                })
                .AddTo(_disposable);

            _volumeFacade.muteBgm
                .Subscribe(_ => _bgmUseCase.SwitchMute())
                .AddTo(_disposable);

            _volumeFacade.muteSe
                .Subscribe(_ => _seUseCase.SwitchMute())
                .AddTo(_disposable);

            _bgmUseCase.isThisMute
                .Subscribe(_volumeFacade.ActivateBgmMute)
                .AddTo(_disposable);

            _seUseCase.isThisMute
                .Subscribe(_volumeFacade.ActivateSeMute)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}