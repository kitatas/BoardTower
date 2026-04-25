using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View;

namespace BoardTower.Common.Presentation.Facade
{
    public sealed class BgmFacade
    {
        private readonly SoundView _soundView;

        public BgmFacade(SoundView soundView)
        {
            _soundView = soundView;
        }

        public void Play(BgmSoundVO sound)
        {
            _soundView.PlayBgm(sound);
        }
    }
}