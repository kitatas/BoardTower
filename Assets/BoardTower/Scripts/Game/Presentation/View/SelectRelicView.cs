using UniEx;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SelectRelicView : MonoBehaviour
    {
        [SerializeField] private Image highlight = default;

        public bool IsEqualPosition(Vector3 position)
        {
            var pos = transform.position;
            return pos.x.IsEqual(position.x) && pos.y.IsEqual(position.y) && pos.z.IsEqual(position.z);
        }

        public void SetPosition(Vector3 position)
        {
            highlight.transform.position = position;
        }
    }
}