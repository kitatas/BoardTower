using UnityEngine;

namespace BoardTower.Common.Utility
{
    public static class RectTransformHelper
    {
        public static (int, int, int, int) GetMargins(this RectTransform self, Canvas canvas)
        {
            var corners = new Vector3[4];
            self.GetWorldCorners(corners);

            Vector3 bottomLeft = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[0]);
            Vector3 topRight = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[2]);

            return (
                Mathf.RoundToInt(bottomLeft.x),
                Mathf.RoundToInt(Screen.height - topRight.y),
                Mathf.RoundToInt(Screen.width - topRight.x),
                Mathf.RoundToInt(bottomLeft.y)
            );
        }
    }
}