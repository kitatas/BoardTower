using System;
using BoardTower.Common.Application;

namespace BoardTower.Common.Domain.Repository
{
    public interface ISoundRepository
    {
        BgmAudioVO Find(BgmType type);
        SeAudioVO Find(SeType type);
        AudioVO<T> Find<T>(T type) where T : Enum;
    }
}