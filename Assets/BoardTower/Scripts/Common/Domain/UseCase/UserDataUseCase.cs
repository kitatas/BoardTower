using BoardTower.Common.Domain.Repository;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class UserDataUseCase
    {
        private readonly SaveRepository _saveRepository;

        public UserDataUseCase(SaveRepository saveRepository)
        {
            _saveRepository = saveRepository;
        }

        public void Delete()
        {
            _saveRepository.Delete();
        }
    }
}