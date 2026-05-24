using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Repository;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class LoginUseCase
    {
        private readonly UserEntity _userEntity;
        private readonly SaveRepository _saveRepository;

        public LoginUseCase(UserEntity userEntity, SaveRepository saveRepository)
        {
            _userEntity = userEntity;
            _saveRepository = saveRepository;
        }

        public async UniTask LoginAsync(CancellationToken token)
        {
            var user = await FetchUserAsync(token);
            _userEntity.Set(new UserVO(user, default));
        }

        private async UniTask<LocalUserVO> FetchUserAsync(CancellationToken token)
        {
            var saveData = await _saveRepository.LoadAsync(token);
            if (string.IsNullOrEmpty(saveData.user.id))
            {
                return CreateUser();
            }
            else
            {
                return saveData.user;
            }
        }

        private LocalUserVO CreateUser()
        {
            var id = Ulid.NewUlid().ToString();
            var user = new LocalUserVO(id);
            _saveRepository.SaveUser(user);
            return user;
        }
    }
}