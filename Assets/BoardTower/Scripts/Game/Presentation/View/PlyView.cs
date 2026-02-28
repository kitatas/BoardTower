using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class PlyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI plyCount = default;

        public Tween Render(int prev, int current, float duration)
        {
            return DOTween.To(
                    () => prev,
                    x => plyCount.text = ZString.Format("{0:N0}", x),
                    current,
                    duration
                )
                .SetLink(gameObject);
        }
    }
}