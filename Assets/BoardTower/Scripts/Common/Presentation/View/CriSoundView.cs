using BoardTower.Common.Application;
using CriWare;
using UniEx;
using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class CriSoundView : MonoBehaviour, ISoundView
    {
        [SerializeField] private CriAtomSource bgmSource = default;
        [SerializeField] private CriAtomSource seSource = default;

        public void PlayBgm(BgmSoundVO sound)
        {
            if (bgmSource.cueName == sound.audio.cue) return;
            bgmSource.cueName = sound.audio.cue;

            if (sound.isMute) return;

            this.Delay(sound.delay, () =>
            {
                if (bgmSource.status is CriAtomSourceBase.Status.Playing) bgmSource.Stop();
                bgmSource.Play();
            });
        }

        public void PlaySe(SeSoundVO sound)
        {
            seSource.cueName = sound.audio.cue;

            if (sound.isMute) return;

            this.Delay(sound.delay, () =>
            {
                seSource.Play();
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

        public void PauseBgm()
        {
            bgmSource.Pause(true);
        }

        public void UnPauseBgm()
        {
            bgmSource.Pause(false);
        }
    }
}