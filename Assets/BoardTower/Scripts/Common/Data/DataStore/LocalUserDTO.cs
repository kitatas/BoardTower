using System;
using BoardTower.Common.Application;

namespace BoardTower.Common.Data.DataStore
{
    [Serializable]
    public sealed class LocalUserDTO
    {
        public string id;

        public LocalUserDTO()
        {
            id = "";
        }

        public LocalUserDTO(LocalUserVO user)
        {
            id = user.id;
        }

        public LocalUserVO ToVO() => new(id);
    }
}