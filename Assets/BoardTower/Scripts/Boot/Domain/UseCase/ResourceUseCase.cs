using System.Threading;
using BoardTower.Common.Domain.Repository;
using Cysharp.Threading.Tasks;

namespace BoardTower.Boot.Domain.UseCase
{
    public sealed class ResourceUseCase
    {
        private readonly LocaleRepository _localeRepository;

        public ResourceUseCase(LocaleRepository localeRepository)
        {
            _localeRepository = localeRepository;
        }

        public UniTask LoadAsync(CancellationToken token)
        {
            return _localeRepository.LoadAsync(token);
        }
    }
}