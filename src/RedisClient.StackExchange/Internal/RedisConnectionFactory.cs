using Microsoft.Extensions.Options;
using RedisClient.Abstractions;
using RedisClient.Models.Options;
using StackExchange.Redis;

namespace RedisClient.StackExchange.Internal
{
    internal class RedisConnectionFactory : IRedisConnectionFactory<ConnectionMultiplexer>, IDisposable
    {
        private readonly SemaphoreSlim _createConnSemaphore = new(1, 1);

        private ConnectionMultiplexer? _redisConnection;

        private readonly RedisOptions _options;

        public RedisConnectionFactory(IOptionsMonitor<RedisOptions> optionsMonitor)
        {
            this._options = optionsMonitor.CurrentValue;
        }

        public async Task<ConnectionMultiplexer> CreateAsync(CancellationToken cancellationToken = default)
        {
            if (_redisConnection != null)
            {
                return _redisConnection;
            }

            await _createConnSemaphore.WaitAsync(cancellationToken);
            _redisConnection ??= await ConnectionMultiplexer.ConnectAsync($"{_options.Host}:{_options.Port}",
                opt => { opt.Password = _options.Password; });
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
            _redisConnection ??=
                ConnectionMultiplexer.Connect($"{_options.Host}:{_options.Port}", opt => { opt.Password = _options.Password; });
            _createConnSemaphore.Release();

            return _redisConnection;
        }

        public void Dispose()
        {
            _createConnSemaphore.Dispose();
            _redisConnection?.Dispose();
        }
    }
}
