using BoardTower.Game.Presentation.View.Button;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View
{
    public sealed class VolumeItemView : MonoBehaviour
    {
        [SerializeField] private Slider slider = default;
        [SerializeField] private MuteButtonView muteButton = default;

        public Observable<float> volume => slider.OnValueChangedAsObservable();
        public Observable<Unit> release => slider.OnPointerUpAsObservable().Select(_ => Unit.Default);
        public Observable<Unit> mute => muteButton.click.Select(_ => Unit.Default);

        public void SetVolume(float value)
        {
            slider.value = value;
        }

        public void ActivateMute(bool value)
        {
            muteButton.ActivateMute(value);
        }
    }
}