using Cysharp.Text;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class UidView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI uid = default;

        public void Render(string value)
        {
            uid.text = ZString.Format("ID:{0}", value);
        }
    }
}