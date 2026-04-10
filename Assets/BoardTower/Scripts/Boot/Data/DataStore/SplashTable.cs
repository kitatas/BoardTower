using BoardTower.Common.Data.DataStore;
using UnityEngine;

namespace BoardTower.Boot.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(SplashTable), menuName = "DataTable/" + nameof(SplashTable))]
    public sealed class SplashTable : BaseTable<SplashData>
    {
    }
}