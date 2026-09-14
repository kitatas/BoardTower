using BoardTower.Common.Presentation.View.Button;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View.Button
{
    public sealed class MuteButtonView : BaseButtonView
    {
        [SerializeField] private Image activeIcon = default;
        [SerializeField] private Image inactiveIcon = default;

        public void ActivateMute(bool value)
        {
            activeIcon.enabled = !value;
            inactiveIcon.enabled = value;
        }
    }
}