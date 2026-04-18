using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View.Button;
using BoardTower.Game.Application;

namespace BoardTower.Game.Presentation.View.Button
{
    public sealed class GameModalButtonView : BaseModalButtonView<GameModalType>
    {
        protected override BaseModalVO<GameModalType> modal => new GameModalVO(modalType, fadeType);
    }
}