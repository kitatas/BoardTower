using System.Collections.Generic;
using BoardTower.Common.Application;
using Newtonsoft.Json;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class PlayFabTitleData
    {
        private Dictionary<string, string> _titleData;

        public void CacheTitleData(Dictionary<string, string> titleData)
        {
            _titleData = titleData;
        }

        public IList<T> Deserialize<T>(string key)
        {
            if (_titleData == null) throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_DESERIALIZE_MASTER);

            return _titleData.TryGetValue(key, out var master)
                ? JsonConvert.DeserializeObject<T[]>(master)
                : throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_DESERIALIZE_MASTER);
        }
    }
}