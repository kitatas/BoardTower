using BoardTower.Game.Presentation.View;
using R3;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class VolumeFacade
    {
        private readonly VolumeView _volumeView;

        public VolumeFacade(VolumeView volumeView)
        {
            _volumeView = volumeView;
        }

        public Observable<float> masterVolume => _volumeView.masterVolume;
        public Observable<float> bgmVolume => _volumeView.bgmVolume;
        public Observable<float> seVolume => _volumeView.seVolume;
        public Observable<Unit> releaseMaster => _volumeView.releaseMaster;
        public Observable<Unit> releaseBgm => _volumeView.releaseBgm;
        public Observable<Unit> releaseSe => _volumeView.releaseSe;
        public Observable<Unit> releaseAny => releaseMaster.Merge(releaseBgm).Merge(releaseSe);
        public Observable<Unit> muteMaster => _volumeView.muteMaster;
        public Observable<Unit> muteBgm => _volumeView.muteBgm;
        public Observable<Unit> muteSe => _volumeView.muteSe;

        public void Init(float masterValue, float bgmValue, float seValue)
        {
            _volumeView.Init(masterValue, bgmValue, seValue);
        }

        public void ActivateMasterMute(bool value)
        {
            _volumeView.ActivateMasterMute(value);
        }

        public void ActivateBgmMute(bool value)
        {
            _volumeView.ActivateBgmMute(value);
        }

        public void ActivateSeMute(bool value)
        {
            _volumeView.ActivateSeMute(value);
        }
    }
}