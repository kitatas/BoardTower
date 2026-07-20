using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;
using Newtonsoft.Json;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class PlayFabMasterDTO
    {
        public readonly Dictionary<string, string> titleData;

        public PlayFabMasterDTO(Dictionary<string, string> titleData)
        {
            this.titleData = titleData;
        }

        public PlayFabMasterVO ToVO() => new(
            DeserializeMaster<AchievementDTO>(PlayFabConfig.ACHIEVEMENT_KEY).Select(x => x.ToVO())
        );

        private IEnumerable<T> DeserializeMaster<T>(string key)
        {
            return titleData.TryGetValue(key, out var master)
                ? JsonConvert.DeserializeObject<T[]>(master)
                : throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_DESERIALIZE_MASTER);
        }
    }
}