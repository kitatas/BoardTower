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

        public Observable<float> bgmVolume => _volumeView.bgmVolume;
        public Observable<float> seVolume => _volumeView.seVolume;
        public Observable<Unit> releaseBgm => _volumeView.releaseBgm;
        public Observable<Unit> releaseSe => _volumeView.releaseSe;
        public Observable<Unit> releaseAny => releaseBgm.Merge(releaseSe);
        public Observable<Unit> muteBgm => _volumeView.muteBgm;
        public Observable<Unit> muteSe => _volumeView.muteSe;

        public void Init(float bgmValue, float seValue)
        {
            _volumeView.Init(bgmValue, seValue);
        }
    }
}