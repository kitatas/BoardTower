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
        private readonly MasterEntity _masterEntity;
        private readonly UserEntity _userEntity;
        private readonly PlayFabRepository _playFabRepository;
        private readonly SaveRepository _saveRepository;

        public LoginUseCase(MasterEntity masterEntity, UserEntity userEntity, PlayFabRepository playFabRepository,
            SaveRepository saveRepository)
        {
            _masterEntity = masterEntity;
            _userEntity = userEntity;
            _playFabRepository = playFabRepository;
            _saveRepository = saveRepository;
        }

        public async UniTask<LoginResultVO> LoginAsync(CancellationToken token)
        {
            var (master, user) = await FetchUserAsync(token);
            _masterEntity.Set(master);
            _userEntity.Set(user);
            return new LoginResultVO(true, _userEntity.isRegistered);
        }

        private async UniTask<(MasterVO, UserVO)> FetchUserAsync(CancellationToken token)
        {
            var saveData = await _saveRepository.LoadAsync(token);
            if (string.IsNullOrEmpty(saveData.user.id))
            {
                return await CreateUserAsync(token);
            }
            else
            {
                var uid = saveData.user.id;
                var (playFabMaster, playFabUser) = await _playFabRepository.LoginAsync(uid, token);
                return (
                    new MasterVO(playFabMaster),
                    new UserVO(saveData.user, playFabUser)
                );
            }
        }

        private async UniTask<(MasterVO, UserVO)> CreateUserAsync(CancellationToken token)
        {
            for (int i = 0; i < PlayFabConfig.CREATE_UID_RETRY_COUNT; i++)
            {
                var uid = Ulid.NewUlid().ToString();
                var (playFabMaster, playFabUser) = await _playFabRepository.LoginAsync(uid, token);

                if (playFabUser.isNewly)
                {
                    var localUser = new LocalUserVO(uid);
                    _saveRepository.SaveUser(localUser);
                    return (
                        new MasterVO(playFabMaster),
                        new UserVO(localUser, playFabUser)
                    );
                }
            }

            throw new RebootExceptionVO(ExceptionConfig.FAILED_TO_CREATE_UID);
        }

        public async UniTask RegisterAsync(UserDisplayNameVO userDisplayName, CancellationToken token)
        {
            var displayName = await _playFabRepository.UpdateDisplayNameAsync(userDisplayName.value, token);
            _userEntity.SetDisplayName(displayName);
        }
    }
}