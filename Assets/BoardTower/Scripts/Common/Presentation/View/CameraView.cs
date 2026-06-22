using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Common.Presentation.View
{
    public sealed class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera = default;
        [SerializeField] private CanvasScaler canvasScaler = default;

        private void Awake()
        {
            var resolution = canvasScaler.referenceResolution;
            var r = resolution.y / resolution.x;
            var s = (float)Screen.height / (float)Screen.width;
            var d = s / r;
            if (d > 1.0f) mainCamera.orthographicSize *= d;
        }
    }
}