using BoardTower.Common.Application;
using R3;
using UnityEngine;

namespace BoardTower.Common.Presentation.View.Button
{
    public abstract class BaseButtonView : MonoBehaviour
    {
        [SerializeField] protected UnityEngine.UI.Button button = default;
        [SerializeField] protected SeType seType = default;

        public Observable<SeType> click => button
            .OnClickAsObservable()
            .Select(_ => seType);
    }
}