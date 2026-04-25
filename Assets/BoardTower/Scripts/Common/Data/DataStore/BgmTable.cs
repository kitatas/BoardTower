using UnityEngine;

namespace BoardTower.Common.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(BgmTable), menuName = "DataTable/" + nameof(BgmTable))]
    public sealed class BgmTable : BaseTable<BgmData>
    {
    }
}