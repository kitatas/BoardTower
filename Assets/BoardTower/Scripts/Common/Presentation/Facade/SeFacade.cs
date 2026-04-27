using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View;

namespace BoardTower.Common.Presentation.Facade
{
    public sealed class SeFacade
    {
        private readonly SoundView _soundView;

        public SeFacade(SoundView soundView)
        {
            _soundView = soundView;
        }

        public void Play(SeSoundVO sound)
        {
            _soundView.PlaySe(sound);
        }

        public void SetVolume(float volume)
        {
            _soundView.SetSeVolume(volume);
        }
    }
}