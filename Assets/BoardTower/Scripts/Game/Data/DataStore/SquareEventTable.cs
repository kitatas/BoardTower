using BoardTower.Common.Data.DataStore;
using UnityEngine;

namespace BoardTower.Game.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(SquareEventTable), menuName = "DataTable/" + nameof(SquareEventTable))]
    public sealed class SquareEventTable : BaseTable<SquareEventData>
    {
    }
}