using BoardTower.Game.Application;
using UnityEngine;

namespace BoardTower.Game.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(RelicData), menuName = "DataTable/" + nameof(RelicData))]
    public sealed class RelicData : ScriptableObject
    {
        [SerializeField] private RelicType relicType = default;
        [SerializeField] private string content = default;

        public RelicType type => relicType;
        public RelicVO ToVO() => new(relicType, content);
    }
}