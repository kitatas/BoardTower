using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View.Button;
using BoardTower.Game.Application;

namespace BoardTower.Game.Presentation.View.Button
{
    public sealed class GameModalButtonView : BaseModalButtonView<GameModalType>
    {
        protected override BaseModalVO<GameModalType> modal => new GameModalVO(modalType, fadeType);
        public GameModalType type => modal.type;

        private void Start()
        {
#if UNITY_WEBGL
            if (modalType is GameModalType.Policy or GameModalType.License or GameModalType.Credit)
            {
                SetInteractable(false);
            }
#endif
        }
    }
}