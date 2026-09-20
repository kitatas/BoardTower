using System.Linq;
using BoardTower.Boot.Application;
using BoardTower.Boot.Data.DataStore;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;

namespace BoardTower.Boot.Domain.Repository
{
    public sealed class AppVersionRepository
    {
        private readonly PlayFabTitleData _titleData;

        public AppVersionRepository(PlayFabTitleData titleData)
        {
            _titleData = titleData;
        }

        public AppVersionVO Fetch()
        {
            var dto = _titleData.Deserialize<AppVersionDTO>(PlayFabConfig.APP_VERSION_KEY)?.First();
            if (dto == null) throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_APP_VERSION);

            return dto.ToVO();
        }
    }
}