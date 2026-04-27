using BoardTower.Common.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class SoundView : MonoBehaviour
    {
        [SerializeField] private AudioSource bgmSource = default;
        [SerializeField] private AudioSource seSource = default;

        public void PlayBgm(BgmSoundVO sound)
        {
            this.Delay(sound.delay, () =>
            {
                bgmSource.clip = sound.audio.clip;
                bgmSource.Play();
            });
        }

        public void PlaySe(SeSoundVO sound)
        {
            this.Delay(sound.delay, () =>
            {
                seSource.PlayOneShot(sound.audio.clip);
            });
        }

        public void SetBgmVolume(float volume)
        {
            bgmSource.volume = volume;
        }

        public void SetSeVolume(float volume)
        {
            seSource.volume = volume;
        }
    }
}