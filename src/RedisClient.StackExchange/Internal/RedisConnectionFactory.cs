using Microsoft.Extensions.Options;
using RedisClient.Abstractions;
using RedisClient.Models.Options;
using StackExchange.Redis;

namespace RedisClient.StackExchange.Internal
{
    internal class RedisConnectionFactory : IRedisConnectionFactory<ConnectionMultiplexer>, IDisposable
    {
        private readonly SemaphoreSlim _createConnSemaphore = new SemaphoreSlim(1, 1);

        private ConnectionMultiplexer? _redisConnection;

        private readonly IOptionsMonitor<RedisOptions> _optionsMonitor;

        public RedisConnectionFactory(IOptionsMonitor<RedisOptions> optionsMonitor)
        {
            this._optionsMonitor = optionsMonitor;
        }

        public async Task<ConnectionMultiplexer> CreateAsync(CancellationToken cancellationToken = default)
        {
            if (_redisConnection != null)
            {
                return _redisConnection;
            }

            await _createConnSemaphore.WaitAsync();
            var options = _optionsMonitor.CurrentValue;
            _redisConnection = await ConnectionMultiplexer.ConnectAsync($"{options.Host}:{options.Port}", opt =>
            {
                opt.Password = options.Password;
            });
            _createConnSemaphore.Release();

            return _redisConnection;
        }

        public ConnectionMultiplexer Create()
        {
            if (_redisConnection != null)
            {
                return _redisConnection;
            }

            _createConnSemaphore.Wait();
            var options = _optionsMonitor.CurrentValue;
            _redisConnection = ConnectionMultiplexer.Connect($"{options.Host}:{options.Port}", opt =>
            {
                opt.Password = options.Password;
            });
            _createConnSemaphore.Release();

            return _redisConnection;
        }

        public void Dispose()
        {
            DisposeConnection();
        }

        private void DisposeConnection()
        {
            _redisConnection?.Dispose();
        }
    }
}
