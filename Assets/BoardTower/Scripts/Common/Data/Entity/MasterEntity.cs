using System.Collections.Generic;
using BoardTower.Common.Application;

namespace BoardTower.Common.Data.Entity
{
    public sealed class MasterEntity : BaseEntity<MasterVO>
    {
        public IEnumerable<AchievementVO> achievements => value.playFabMaster.achievements;
    }
}