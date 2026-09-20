using BoardTower.Common.Application;
using BoardTower.Common.Presentation.View.Button;
using R3;
using UnityEngine;

namespace BoardTower.Boot.Presentation.View
{
    public sealed class UpdateView : MonoBehaviour
    {
        [SerializeField] private CommonButtonView commonButtonView = default;

        private void Start()
        {
            commonButtonView.click
                .Subscribe(_ => UnityEngine.Application.OpenURL(UrlConfig.APP_URL))
                .AddTo(this);
        }
    }
}