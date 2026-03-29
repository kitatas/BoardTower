using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class ExceptionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI message = default;

        public Tween Render(string value, float duration)
        {
            return DOTween.Sequence()
                .AppendCallback(() => message.text = value)
                .SetLink(gameObject);
        }
    }
}