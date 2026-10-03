using BoardTower.Common.Application;
using R3;
using UnityEngine;

namespace BoardTower.Common.Presentation.View.Button
{
    public sealed class UrlButtonView : BaseButtonView
    {
        [SerializeField] private UrlType type = default;

        private void Start()
        {
#if UNITY_WEBGL
            SetInteractable(false);
#else
            click
                .Subscribe(_ => UnityEngine.Application.OpenURL(type.ToURL()))
                .AddTo(this);
#endif
        }
    }
}