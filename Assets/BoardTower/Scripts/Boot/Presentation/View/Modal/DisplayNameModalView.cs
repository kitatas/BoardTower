using BoardTower.Boot.Presentation.View.Button;
using R3;
using TMPro;
using UnityEngine;

namespace BoardTower.Boot.Presentation.View.Modal
{
    public sealed class DisplayNameModalView : BaseBootModalView
    {
        [SerializeField] private TMP_InputField inputField = default;
        [SerializeField] private BootModalButtonView decisionButtonView = default;

        public Observable<string> decisionDisplayName => decisionButtonView.click
            .Select(_ => inputField.text);

        private void SetName(string value)
        {
            inputField.text = value;
        }

        protected override void PreFadeIn()
        {
            SetName("");
        }
    }
}