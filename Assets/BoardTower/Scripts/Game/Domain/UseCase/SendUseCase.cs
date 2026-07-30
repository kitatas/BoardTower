using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Repository;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using Cysharp.Threading.Tasks;
using FastEnumUtility;
using UnityEngine;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class SendUseCase
    {
        private readonly ScoreEntity _scoreEntity;
        private readonly UserEntity _userEntity;
        private readonly PlayFabRepository _playFabRepository;

        public SendUseCase(ScoreEntity scoreEntity, UserEntity userEntity, PlayFabRepository playFabRepository)
        {
            _scoreEntity = scoreEntity;
            _userEntity = userEntity;
            _playFabRepository = playFabRepository;
        }

        public UniTask SendScoreAsync(CancellationToken token)
        {
            return _playFabRepository.UpdatePlayerScoreAsync(_scoreEntity.value, token);
        }

        public UniTask UpdateProgressAsync(bool isClear, CancellationToken token)
        {
            var progresses = new ProgressVO[AchievementConfig.ACHIEVEMENTS.Length];
            for (int i = 0; i < AchievementConfig.ACHIEVEMENTS.Length; i++)
            {
                var progress = _userEntity.Find(AchievementConfig.ACHIEVEMENTS[i].ToInt32());
                progresses[i] = progress.type.ToAchievementType() switch
                {
                    AchievementType.Play => new(progress.type, progress.value + 1),
                    AchievementType.Score => new(progress.type, Mathf.Max(progress.value, _scoreEntity.value)),
                    AchievementType.Clear => new(progress.type, progress.value + (isClear ? 1 : 0)),
                    _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_ACHIEVEMENT),
                };
            }

            _userEntity.UpdateProgress(progresses);
            return _playFabRepository.UpdateProgressesAsync(_userEntity.value.playFabUser.progresses, token);
        }
    }
}