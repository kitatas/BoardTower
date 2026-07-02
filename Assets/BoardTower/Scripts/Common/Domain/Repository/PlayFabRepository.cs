using System.Collections.Generic;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using Cysharp.Threading.Tasks;
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

        public async UniTask<(PlayFabMasterVO, PlayFabUserVO)> LoginAsync(string uid, CancellationToken token)
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

            var master = FetchMaster(response);
            var user = FetchUser(response);
            return (master.ToVO(), user.ToVO());
        }

        private static PlayFabMasterDTO FetchMaster(LoginResult loginResult)
        {
            var payload = loginResult.InfoResultPayload;
            if (payload == null) throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_FETCH_PAYLOAD);

            var titleData = payload.TitleData;
            if (titleData == null) throw new QuitExceptionVO(ExceptionConfig.FAILED_TO_FETCH_MASTER);

            return new PlayFabMasterDTO(titleData);
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

        public UniTask SendScoreRankingAsync(int score, CancellationToken token)
        {
            return SendRankingAsync(PlayFabConfig.SCORE_RANKING_KEY, score, token);
        }

        private async UniTask SendRankingAsync(string key, int score, CancellationToken token)
        {
            if (_playFabSession == null) return;

            var completionSource = new UniTaskCompletionSource<PlayFab.ProgressionModels.EmptyResponse>();
            var request = new UpdateLeaderboardEntriesRequest
            {
                Entries = new List<LeaderboardEntryUpdate>
                {
                    new()
                    {
                        EntityId = _playFabSession.entityId,
                        Scores = new List<string> { score.ToString() },
                    },
                },
                LeaderboardName = key,
            };

            _playFabSession.UpdateLeaderboardEntries(
                request,
                result => completionSource.TrySetResult(result),
                error => completionSource.TrySetException(new RebootExceptionVO(error.ErrorMessage))
            );

            await completionSource.Task.AttachExternalCancellation(token);
        }
    }
}