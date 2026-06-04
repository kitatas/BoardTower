using BoardTower.Common.Utility;
using BoardTower.Game.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View.Modal
{
    public sealed class GameModalWebviewView : BaseGameModalView
    {
        [SerializeField] private RectTransform view = default;
        private static WebViewObject _webViewObject = null;

        protected override void PreFadeIn()
        {
            _webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();
            _webViewObject.Init(enableWKWebView: true);

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            _webViewObject.bitmapRefreshCycle = 1;
#endif

            var margins = view.GetMargins();
            _webViewObject.SetMargins(margins.left, margins.top, margins.right, margins.bottom);
            _webViewObject.LoadURL(type.ToURL());
        }

        protected override void PostFadeIn()
        {
            this.Delay(0.05f, () =>
            {
                // ポップアップ完了後に表示する
                _webViewObject.SetVisibility(true);
            });
        }

        protected override void PreFadeOut()
        {
            if (_webViewObject == null) return;

            _webViewObject.SetVisibility(false);
            Destroy(_webViewObject.gameObject);
        }
    }
}