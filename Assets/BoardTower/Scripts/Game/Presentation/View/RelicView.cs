using BoardTower.Game.Application;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class RelicView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI content = default;

        public void Render(RelicVO value)
        {
            content.text = value.content;
        }
    }
}