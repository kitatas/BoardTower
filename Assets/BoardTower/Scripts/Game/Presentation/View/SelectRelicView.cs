using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SelectRelicView : MonoBehaviour
    {
        [SerializeField] private Image highlight = default;

        public void SetPosition(Vector3 position)
        {
            highlight.transform.position = position;
        }
    }
}