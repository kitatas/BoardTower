using BoardTower.Boot.Application;
using BoardTower.Boot.Domain.Repository;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class AppVersionUseCase
    {
        private readonly AppVersionRepository _appVersionRepository;

        public AppVersionUseCase(AppVersionRepository appVersionRepository)
        {
            _appVersionRepository = appVersionRepository;
        }

        public bool IsForceUpdate()
        {
            var local = GetLocalVersion();
            var remote = _appVersionRepository.Fetch();
            return remote.major > local.major || remote.major == local.major && remote.minor > local.minor;
        }

        private static AppVersionVO GetLocalVersion()
        {
            var versions = UnityEngine.Application.version.Split('.');
            var major = int.Parse(versions[0]);
            var minor = int.Parse(versions[1]);
            return new AppVersionVO(major, minor);
        }
    }
}