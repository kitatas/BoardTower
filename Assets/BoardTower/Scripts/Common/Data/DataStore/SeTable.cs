using UnityEngine;

namespace BoardTower.Common.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(SeTable), menuName = "DataTable/" + nameof(SeTable))]
    public sealed class SeTable : BaseTable<SeData>
    {
    }
}