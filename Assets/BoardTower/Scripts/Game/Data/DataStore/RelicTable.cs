using BoardTower.Common.Data.DataStore;
using UnityEngine;

namespace BoardTower.Game.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(RelicTable), menuName = "DataTable/" + nameof(RelicTable))]
    public sealed class RelicTable : BaseTable<RelicData>
    {
    }
}