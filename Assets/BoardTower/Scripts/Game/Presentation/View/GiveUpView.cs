using BoardTower.Common.Presentation.View.Button;
using R3;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class GiveUpView : MonoBehaviour
    {
        [SerializeField] private BaseButtonView decision = default;
        [SerializeField] private BaseButtonView backTop = default;

        public Observable<Unit> clickDecision => decision.click.Select(_ => Unit.Default);
        public Observable<Unit> clickBackTop => backTop.click.Select(_ => Unit.Default);
    }
}