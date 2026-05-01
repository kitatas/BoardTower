using BoardTower.Common.Presentation.View.Button;
using R3;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class DeleteView : MonoBehaviour
    {
        [SerializeField] private BaseButtonView decision = default;
        [SerializeField] private BaseButtonView backTitle = default;

        public Observable<Unit> clickDecision => decision.click.Select(_ => Unit.Default);
        public Observable<Unit> clickBackTitle => backTitle.click.Select(_ => Unit.Default);
    }
}