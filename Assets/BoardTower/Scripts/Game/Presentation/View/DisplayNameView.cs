using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class DisplayNameView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField = default;

        public void Render(string displayName)
        {
            inputField.text = displayName;
        }
    }
}