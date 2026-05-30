using BoardTower.Common.Application;

namespace BoardTower.Common.Data.Entity
{
    public sealed class RetryCountEntity : BaseEntity<int>
    {
        public void Reset() => Set(0);
        public void Increment() => Set(value + 1);

        public void Update(bool isEqual)
        {
            if (isEqual)
            {
                Increment();
            }
            else
            {
                Set(1);
            }
        }

        public bool IsMaxRetry() => value > ExceptionConfig.MAX_RETRY_COUNT;
    }
}