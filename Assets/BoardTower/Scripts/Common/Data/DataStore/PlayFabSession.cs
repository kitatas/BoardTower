using System;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ProgressionModels;

namespace BoardTower.Common.Data.DataStore
{
    public sealed class PlayFabSession
    {
        private readonly PlayFabAuthenticationContext _playFabAuthenticationContext;
        private readonly PlayFabProgressionInstanceAPI _playFabProgressionInstanceAPI;

        public PlayFabSession(LoginResult loginResult)
        {
            _playFabAuthenticationContext = new PlayFabAuthenticationContext(
                loginResult.SessionTicket,
                loginResult.EntityToken.EntityToken,
                loginResult.PlayFabId,
                loginResult.EntityToken.Entity.Id,
                loginResult.EntityToken.Entity.Type
            );
            _playFabProgressionInstanceAPI = new PlayFabProgressionInstanceAPI(_playFabAuthenticationContext);
        }

        public string entityId => _playFabAuthenticationContext.EntityId;

        public void UpdateLeaderboardEntries(UpdateLeaderboardEntriesRequest request,
            Action<PlayFab.ProgressionModels.EmptyResponse> result, Action<PlayFabError> error)
        {
            _playFabProgressionInstanceAPI.UpdateLeaderboardEntries(request, result, error);
        }
    }
}