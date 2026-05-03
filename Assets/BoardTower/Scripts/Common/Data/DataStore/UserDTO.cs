using System;
using BoardTower.Common.Application;

namespace BoardTower.Common.Data.DataStore
{
    [Serializable]
    public sealed class UserDTO
    {
        public string id;

        public UserDTO()
        {
            id = "";
        }

        public UserDTO(UserVO user)
        {
            id = user.id;
        }

        public UserVO ToVO() => new(id);
    }
}