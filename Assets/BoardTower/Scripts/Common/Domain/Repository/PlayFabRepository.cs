using System.Collections.Generic;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ProgressionModels;

namespace BoardTower.Common.Domain.Repository
{
    public sealed class PlayFabRepository
    {
        private PlayFabSession _playFabSession;

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
                    GetTitleData = true,
                },
            };

            PlayFabClientAPI.LoginWithCustomID(
                request,
                result => completionSource.TrySetResult(result),
                error => completionSource.TrySetException(new RebootExceptionVO(error.ErrorMessage))
            );

            var response = await completionSource.Task.AttachExternalCancellation(token);
            _playFabSession = new PlayFabSession(response);

            var user = FetchUser(response);
            return user.ToVO();
        }

        private static PlayFabUserDTO FetchUser(LoginResult loginResult)
        {
            var payload = loginResult.InfoResultPayload;
            if (payload == null) throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_FETCH_PAYLOAD);

            var records = payload.UserData;
            if (records == null) throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_FETCH_RECORD);

            var name = payload.PlayerProfile?.DisplayName ?? "";
            var displayName = string.IsNullOrEmpty(name) ? UserDisplayNameVO.Create() : new UserDisplayNameVO(name);
            return new PlayFabUserDTO(loginResult, displayName, records);
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

        public UniTask UpdatePlayerScoreAsync(int score, CancellationToken token)
        {
            return UpdatePlayerStatisticsAsync(PlayFabConfig.SCORE_KEY, score, token);
        }

        private async UniTask UpdatePlayerStatisticsAsync(string key, int score, CancellationToken token)
        {
            if (_playFabSession == null) return;

            var completionSource = new UniTaskCompletionSource<UpdateStatisticsResponse>();
            var request = new UpdateStatisticsRequest
            {
                Entity = new PlayFab.ProgressionModels.EntityKey
                {
                    Id = _playFabSession.entityId,
                    Type = _playFabSession.entityType,
                },
                Statistics = new List<PlayFab.ProgressionModels.StatisticUpdate>
                {
                    new()
                    {
                        Name = key,
                        Scores = new List<string> { score.ToString() },
                    },
                },
            };

            _playFabSession.UpdateStatistics(
                request,
                result => completionSource.TrySetResult(result),
                error => completionSource.TrySetException(new RebootExceptionVO(error.ErrorMessage))
            );

            await completionSource.Task.AttachExternalCancellation(token);
        }

        public UniTask UpdateProgressesAsync(ProgressVO[] progresses, CancellationToken token)
        {
            var json = JsonConvert.SerializeObject(progresses);
            return UpdateUserDataAsync(PlayFabConfig.PROGRESS_KEY, json, token);
        }

        private static async UniTask UpdateUserDataAsync(string key, string json, CancellationToken token)
        {
            var completionSource = new UniTaskCompletionSource<UpdateUserDataResult>();
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { key, json },
                },
            };

            PlayFabClientAPI.UpdateUserData(
                request,
                result => completionSource.TrySetResult(result),
                error => completionSource.TrySetException(new RetryExceptionVO(error.ErrorMessage))
            );

            await completionSource.Task.AttachExternalCancellation(token);
        }
    }
}