using System.Collections.Generic;
using System.Linq;
using BoardTower.Boot.Application;
using BoardTower.Boot.Data.DataStore;
using BoardTower.Common.Application;

namespace BoardTower.Boot.Domain.Repository
{
    public sealed class SplashRepository
    {
        private readonly Dictionary<SplashType, SplashVO> _splashMap;

        public SplashRepository(SplashTable splashTable)
        {
            _splashMap = splashTable.all.ToDictionary(x => x.type, x => x.ToVO());
        }

        public SplashVO Find(SplashType type)
        {
            if (_splashMap.TryGetValue(type, out var vo))
            {
                return vo;
            }
            else
            {
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SPLASH);
            }
        }
    }
}