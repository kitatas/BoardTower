using UnityEngine;

namespace BoardTower.Common.Utility
{
    public static class RectTransformHelper
    {
        public static (int left, int top, int right, int bottom) GetMargins(this RectTransform self)
        {
            var corners = new Vector3[4];
            self.GetWorldCorners(corners);

            return (
                Mathf.RoundToInt(corners[0].x),
                Mathf.RoundToInt(Screen.height - corners[2].y),
                Mathf.RoundToInt(Screen.width - corners[2].x),
                Mathf.RoundToInt(corners[0].y)
            );
        }

        public static (int left, int top, int right, int bottom) GetMargins(this RectTransform self, Canvas canvas)
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