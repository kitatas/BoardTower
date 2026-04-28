using BoardTower.Common.Presentation.View.Button;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View.Button
{
    public sealed class MuteButtonView : BaseButtonView
    {
        [SerializeField] private Image icon = default;

        public void ActivateMute(bool value)
        {
            icon.color = value ? Color.lightSlateGray : Color.darkRed;
        }
    }
}