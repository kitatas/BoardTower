using Cysharp.Text;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class GemComboView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI combo = default;

        public void Render(int value)
        {
            combo.text = ZString.Format("{0}Combo", value);
        }

        public void RenderReset()
        {
            combo.text = "";
        }
    }
}