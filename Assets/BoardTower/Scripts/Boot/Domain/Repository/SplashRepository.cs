using System.Collections.Generic;
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
            _splashMap = new Dictionary<SplashType, SplashVO>();
            foreach (var s in splashTable.records) _splashMap.TryAdd(s.type, s.ToVO());
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