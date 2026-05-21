using R3;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class VolumeView : MonoBehaviour
    {
        [SerializeField] private VolumeItemView master = default;
        [SerializeField] private VolumeItemView bgm = default;
        [SerializeField] private VolumeItemView se = default;

        public Observable<float> masterVolume => master.volume;
        public Observable<float> bgmVolume => bgm.volume;
        public Observable<float> seVolume => se.volume;

        public Observable<Unit> releaseMaster => master.release;
        public Observable<Unit> releaseBgm => bgm.release;
        public Observable<Unit> releaseSe => se.release;

        public Observable<Unit> muteMaster => master.mute;
        public Observable<Unit> muteBgm => bgm.mute;
        public Observable<Unit> muteSe => se.mute;

        public void Init(float masterValue, float bgmValue, float seValue)
        {
            master.SetVolume(masterValue);
            bgm.SetVolume(bgmValue);
            se.SetVolume(seValue);
        }

        public void ActivateMasterMute(bool value)
        {
            master.ActivateMute(value);
        }

        public void ActivateBgmMute(bool value)
        {
            bgm.ActivateMute(value);
        }

        public void ActivateSeMute(bool value)
        {
            se.ActivateMute(value);
        }
    }
}