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
        private readonly PlayFabRepository _playFabRepository;
        private readonly SaveRepository _saveRepository;

        public LoginUseCase(UserEntity userEntity, PlayFabRepository playFabRepository, SaveRepository saveRepository)
        {
            _userEntity = userEntity;
            _playFabRepository = playFabRepository;
            _saveRepository = saveRepository;
        }

        public async UniTask<LoginResultVO> LoginAsync(CancellationToken token)
        {
            var user = await FetchUserAsync(token);
            _userEntity.Set(user);
            return new LoginResultVO(true, _userEntity.isRegistered);
        }

        private async UniTask<UserVO> FetchUserAsync(CancellationToken token)
        {
            var saveData = await _saveRepository.LoadAsync(token);
            if (string.IsNullOrEmpty(saveData.user.id))
            {
                return await CreateUserAsync(token);
            }
            else
            {
                var uid = saveData.user.id;
                var playFabUser = await _playFabRepository.LoginAsync(uid, token);
                return new UserVO(saveData.user, playFabUser);
            }
        }

        private async UniTask<UserVO> CreateUserAsync(CancellationToken token)
        {
            for (int i = 0; i < PlayFabConfig.CREATE_UID_RETRY_COUNT; i++)
            {
                var uid = Ulid.NewUlid().ToString();
                var playFabUser = await _playFabRepository.LoginAsync(uid, token);

                if (playFabUser.isNewly)
                {
                    var localUser = new LocalUserVO(uid);
                    _saveRepository.SaveUser(localUser);
                    return new UserVO(localUser, playFabUser);
                }
            }

            throw new RebootExceptionVO(ExceptionConfig.FAILED_TO_CREATE_UID);
        }

        public UniTask RegisterAsync(UserDisplayNameVO userDisplayName, CancellationToken token)
        {
            return _playFabRepository.UpdateDisplayNameAsync(userDisplayName.value, token);
        }
    }
}