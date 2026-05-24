using System.Collections.Generic;
using BoardTower.Common.Application;
using PlayFab.ClientModels;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class PlayFabUserDTO
    {
        public readonly bool isNewly;
        public readonly string displayName;
        public readonly Dictionary<string, UserDataRecord> records;

        public PlayFabUserDTO(bool isNewly, string displayName, Dictionary<string, UserDataRecord> records)
        {
            this.isNewly = isNewly;
            this.displayName = displayName;
            this.records = records;
        }

        public PlayFabUserVO ToVO() => new(isNewly, displayName);
    }
}