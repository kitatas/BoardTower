using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class RoundClearGemCountView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gemCount = default;

        public Tween Render(int prev, int current, float duration)
        {
            return DOTween.To(
                    () => prev,
                    x => gemCount.text = ZString.Format("{0:N0}", x),
                    current,
                    duration
                )
                .SetLink(gameObject);
        }
    }
}