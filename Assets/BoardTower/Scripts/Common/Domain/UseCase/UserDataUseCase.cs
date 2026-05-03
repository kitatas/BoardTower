using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Repository;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class UserDataUseCase
    {
        private readonly UserEntity _userEntity;
        private readonly SaveRepository _saveRepository;

        public UserDataUseCase(UserEntity userEntity, SaveRepository saveRepository)
        {
            _userEntity = userEntity;
            _saveRepository = saveRepository;
        }

        public UserVO user => _userEntity.value;

        public void Delete()
        {
            _saveRepository.Delete();
        }
    }
}