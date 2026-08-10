using System.Collections.Generic;
using BoardTower.Common.Application;
using Newtonsoft.Json;
using PlayFab.ClientModels;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class PlayFabUserDTO
    {
        public readonly LoginResult loginResult;
        public readonly UserDisplayNameVO displayName;
        public readonly Dictionary<string, UserDataRecord> records;

        public PlayFabUserDTO(LoginResult loginResult, UserDisplayNameVO displayName,
            Dictionary<string, UserDataRecord> records)
        {
            this.loginResult = loginResult;
            this.displayName = displayName;
            this.records = records;
        }

        private T Fetch<T>(string key) => records.TryGetValue(key, out var record)
            ? JsonConvert.DeserializeObject<T>(record.Value)
            : default;

        private ProgressVO[] progresses => Fetch<ProgressVO[]>(PlayFabConfig.PROGRESS_KEY);

        public PlayFabUserVO ToVO() => new(
            loginResult.EntityToken.Entity.Id,
            loginResult.NewlyCreated,
            displayName,
            progresses
        );
    }
}