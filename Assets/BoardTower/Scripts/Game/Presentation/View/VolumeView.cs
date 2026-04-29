using BoardTower.Game.Presentation.View.Button;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View
{
    public sealed class VolumeView : MonoBehaviour
    {
        [SerializeField] private Slider master = default;
        [SerializeField] private Slider bgm = default;
        [SerializeField] private Slider se = default;
        [SerializeField] private MuteButtonView masterMute = default;
        [SerializeField] private MuteButtonView bgmMute = default;
        [SerializeField] private MuteButtonView seMute = default;

        public Observable<float> masterVolume => master.OnValueChangedAsObservable();
        public Observable<float> bgmVolume => bgm.OnValueChangedAsObservable();
        public Observable<float> seVolume => se.OnValueChangedAsObservable();

        public Observable<Unit> releaseMaster => master.OnPointerUpAsObservable().Select(_ => Unit.Default);
        public Observable<Unit> releaseBgm => bgm.OnPointerUpAsObservable().Select(_ => Unit.Default);
        public Observable<Unit> releaseSe => se.OnPointerUpAsObservable().Select(_ => Unit.Default);

        public Observable<Unit> muteMaster => masterMute.click.Select(_ => Unit.Default);
        public Observable<Unit> muteBgm => bgmMute.click.Select(_ => Unit.Default);
        public Observable<Unit> muteSe => seMute.click.Select(_ => Unit.Default);

        public void Init(float masterValue, float bgmValue, float seValue)
        {
            master.value = masterValue;
            bgm.value = bgmValue;
            se.value = seValue;
        }

        public void ActivateMasterMute(bool value)
        {
            masterMute.ActivateMute(value);
        }

        public void ActivateBgmMute(bool value)
        {
            bgmMute.ActivateMute(value);
        }

        public void ActivateSeMute(bool value)
        {
            seMute.ActivateMute(value);
        }
    }
}