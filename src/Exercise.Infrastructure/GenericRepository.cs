using Microsoft.Extensions.Logging;

namespace Exercise.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> SaveAsync(T model);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(ILogger<GenericRepository<T>> logger)
        {
            _logger = logger;
        }

        public async Task<T> SaveAsync(T model)
        {
            _logger.LogDebug("SaveAsync");

            // Note: Simulate persistence
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            return model;
        }
    }
}