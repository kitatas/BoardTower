using R3;
using R3.Triggers;
using UniEx;
using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class TapEffectPoolView : BasePoolView<TapEffectView>
    {
        [SerializeField] private TapEffectView tapEffectView = default;
        private Camera _camera;
        private Camera _mainCamera => _camera ? _camera : _camera = Camera.main;

        public override TapEffectView Create()
        {
            return Instantiate(tapEffectView, transform);
        }

        public override void OnGet(TapEffectView item)
        {
            item.Rent();
        }

        public override void OnRelease(TapEffectView item)
        {
            item.Release();
        }

        private void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    var tapPosition = Input.mousePosition;
                    tapPosition.z = 10.0f;

                    var eff = pool.Get();
                    eff.SetUp(_mainCamera.ScreenToWorldPoint(tapPosition), _mainCamera.transform.rotation);
                    this.Delay(eff.duration + 0.5f, () => pool.Release(eff));
                })
                .AddTo(this);
        }
    }
}