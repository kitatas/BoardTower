using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View
{
    public sealed class VolumeView : MonoBehaviour
    {
        [SerializeField] private Slider bgm = default;
        [SerializeField] private Slider se = default;

        public Observable<float> bgmVolume => bgm.OnValueChangedAsObservable();
        public Observable<float> seVolume => se.OnValueChangedAsObservable();

        public Observable<Unit> releaseHandle => bgm.OnPointerUpAsObservable()
            .Merge(se.OnPointerUpAsObservable())
            .Select(_ => Unit.Default);

        public void Init(float bgmValue, float seValue)
        {
            bgm.value = bgmValue;
            se.value = seValue;
        }
    }
}