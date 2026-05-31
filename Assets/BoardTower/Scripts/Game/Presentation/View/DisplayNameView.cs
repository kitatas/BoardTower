using BoardTower.Common.Presentation.View.Button;
using R3;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class DisplayNameView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField = default;
        [SerializeField] private CommonButtonView commonButtonView = default;

        public Observable<string> decisionDisplayName => commonButtonView.click
            .Select(_ => inputField.text);

        public void Render(string displayName)
        {
            inputField.text = displayName;
        }
    }
}