using BoardTower.Game.Application;
using UnityEngine;

namespace BoardTower.Game.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(SquareEventData), menuName = "DataTable/" + nameof(SquareEventData))]
    public sealed class SquareEventData : ScriptableObject
    {
        [SerializeField] private SquareEventType squareEventType = default;
        [SerializeField] private Sprite sprite = default;

        public SquareEventType type => squareEventType;
        public SquareEventVO ToVO() => new(type, sprite);
    }
}