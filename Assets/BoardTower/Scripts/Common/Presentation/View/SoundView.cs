using BoardTower.Common.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class SoundView : MonoBehaviour
    {
        [SerializeField] private AudioSource bgmSource = default;

        public void PlayBgm(BgmSoundVO sound)
        {
            this.Delay(sound.delay, () =>
            {
                bgmSource.clip = sound.audio.clip;
                bgmSource.Play();
            });
        }
    }
}