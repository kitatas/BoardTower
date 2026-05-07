using UniEx;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SelectRelicView : MonoBehaviour
    {
        [SerializeField] private Image highlight = default;

        public void Render(Vector3 position)
        {
            highlight.transform.position = position;
            highlight.SetColorA(1.0f);
        }

        public void Hide()
        {
            highlight.SetColorA(0.0f);
        }
    }
}