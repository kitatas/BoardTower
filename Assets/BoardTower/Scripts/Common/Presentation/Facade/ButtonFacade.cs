using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View.Button;
using R3;

namespace BoardTower.Common.Presentation.Facade
{
    public sealed class ButtonFacade
    {
        private readonly IEnumerable<BaseButtonView> _buttons;

        public ButtonFacade(IEnumerable<BaseButtonView> buttons)
        {
            _buttons = buttons
                .Where(x => x.isInitialized == false);
        }

        public IEnumerable<Observable<SeType>> OnClickAsObservables()
            => _buttons.Select(x => x.click);

        public void Init()
        {
            foreach (var button in _buttons)
            {
                button.Init();
            }
        }
    }
}