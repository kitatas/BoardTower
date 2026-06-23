using System.Collections.Generic;
using BoardTower.Common.Application;
using PlayFab.ClientModels;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class PlayFabUserDTO
    {
        public readonly LoginResult loginResult;
        public readonly UserDisplayNameVO displayName;
        public readonly Dictionary<string, UserDataRecord> records;

        public PlayFabUserDTO(LoginResult loginResult, UserDisplayNameVO displayName, Dictionary<string, UserDataRecord> records)
        {
            this.loginResult = loginResult;
            this.displayName = displayName;
            this.records = records;
        }

        public PlayFabUserVO ToVO() => new(loginResult.NewlyCreated, displayName);
    }
}