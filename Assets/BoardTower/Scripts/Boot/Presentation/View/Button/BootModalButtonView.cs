using BoardTower.Boot.Application;
using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View.Button;

namespace BoardTower.Boot.Presentation.View.Button
{
    public sealed class BootModalButtonView : BaseModalButtonView<BootModalType>
    {
        protected override BaseModalVO<BootModalType> modal => new BootModalVO(modalType, fadeType);
        public BootModalType type => modal.type;
    }
}