using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;

namespace BoardTower.Common.Domain.Repository
{
    public sealed class PlayFabRepository
    {
        public PlayFabRepository()
        {
            PlayFabSettings.staticSettings.TitleId = PlayFabConfig.TITLE_ID;
        }

        public async UniTask<PlayFabUserVO> LoginAsync(string uid, CancellationToken token)
        {
            var completionSource = new UniTaskCompletionSource<LoginResult>();
            var request = new LoginWithCustomIDRequest
            {
                CustomId = uid,
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetUserData = true,
                    GetPlayerProfile = true,
                },
            };

            PlayFabClientAPI.LoginWithCustomID(
                request,
                result => completionSource.TrySetResult(result),
                error => completionSource.TrySetException(new RebootExceptionVO(error.ErrorMessage))
            );

            var response = await completionSource.Task.AttachExternalCancellation(token);
            var user = Create(response);
            return user.ToVO();
        }

        private static PlayFabUserDTO Create(LoginResult loginResult)
        {
            var payload = loginResult.InfoResultPayload;
            if (payload == null) throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_FETCH_PAYLOAD);

            var records = payload.UserData;
            if (records == null) throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_FETCH_RECORD);

            var name = payload.PlayerProfile?.DisplayName ?? "";
            var displayName = string.IsNullOrEmpty(name) ? UserDisplayNameVO.Create() : new UserDisplayNameVO(name);
            return new PlayFabUserDTO(loginResult.NewlyCreated, displayName, records);
        }

        public async UniTask<UserDisplayNameVO> UpdateDisplayNameAsync(string name, CancellationToken token)
        {
            var completionSource = new UniTaskCompletionSource<UpdateUserTitleDisplayNameResult>();
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = name,
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(
                request,
                result => completionSource.TrySetResult(result),
                error => completionSource.TrySetException(new RetryExceptionVO(error.ErrorMessage))
            );

            var response = await completionSource.Task.AttachExternalCancellation(token);
            return new UserDisplayNameVO(response.DisplayName);
        }
    }
}