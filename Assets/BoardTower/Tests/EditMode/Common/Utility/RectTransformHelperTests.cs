using BoardTower.Common.Utility;
using NUnit.Framework;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Common.Utility
{
    /// <summary>
    /// RectTransformHelper のテスト。
    /// ワールド座標 (200, 300) を中心とする 100x50 の RectTransform を用い、
    /// 左下 (150, 275) / 右上 (250, 325) を基準に Screen サイズからの余白を検証する。
    /// </summary>
    [TestFixture]
    public sealed class RectTransformHelperTests
    {
        private GameObject _rectObject;
        private GameObject _canvasObject;
        private RectTransform _rect;
        private Canvas _canvas;

        [SetUp]
        public void SetUp()
        {
            _rectObject = new GameObject("RectTransformHelperTests_Rect", typeof(RectTransform));
            _rect = _rectObject.GetComponent<RectTransform>();
            _rect.sizeDelta = new Vector2(100.0f, 50.0f);
            _rect.position = new Vector3(200.0f, 300.0f, 0.0f);

            // ScreenSpaceOverlay (worldCamera が null) の場合、WorldToScreenPoint はワールド座標の xy をそのまま返す
            _canvasObject = new GameObject("RectTransformHelperTests_Canvas", typeof(Canvas));
            _canvas = _canvasObject.GetComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_rectObject);
            Object.DestroyImmediate(_canvasObject);
        }

        [Test]
        public void GetMargins_ReturnsMarginsFromScreenEdges()
        {
            var expected = (
                left: 150,
                top: Mathf.RoundToInt(Screen.height - 325.0f),
                right: Mathf.RoundToInt(Screen.width - 250.0f),
                bottom: 275
            );

            var margins = _rect.GetMargins();

            Assert.That(margins, Is.EqualTo(expected));
        }

        [TestCase(200.4f, 150)]
        [TestCase(200.6f, 151)]
        public void GetMargins_RoundsLeftToNearestInteger(float positionX, int expectedLeft)
        {
            _rect.position = new Vector3(positionX, 300.0f, 0.0f);

            var margins = _rect.GetMargins();

            Assert.That(margins.left, Is.EqualTo(expectedLeft));
        }

        [Test]
        public void GetMargins_WithCanvas_ReturnsMarginsFromScreenEdges()
        {
            var expected = (
                left: 150,
                top: Mathf.RoundToInt(Screen.height - 325.0f),
                right: Mathf.RoundToInt(Screen.width - 250.0f),
                bottom: 275
            );

            var margins = _rect.GetMargins(_canvas);

            Assert.That(margins, Is.EqualTo(expected));
        }
    }
}
