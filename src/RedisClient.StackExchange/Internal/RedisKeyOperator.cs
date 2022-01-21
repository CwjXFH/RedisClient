using RedisClient.Abstractions;
using RedisClient.Commons.Extensions;
using RedisClient.StackExchange.Extensions;
using StackExchange.Redis;

namespace RedisClient.StackExchange.Internal
{
    internal class RedisKeyOperator : RedisOperator, IRedisKeyOperator
    {
        public RedisKeyOperator(IDatabase database)
            : base(database) { }

        public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await database.KeyExistsAsync(key);
        }

        public async Task<long> ExistsAsync(ICollection<string> keys, CancellationToken cancellationToken = default)
        {
            var redisKeys = keys.ToRedisKeyCollection();
            return await database.KeyExistsAsync(redisKeys.ToArray());
        }

        public async Task<bool> DeleteAsync(string key, CancellationToken cancellationToken)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await database.KeyDeleteAsync(key);
        }

        public async Task<long> DeleteAsync(ICollection<string> keys, CancellationToken cancellationToken = default)
        {
            if (keys.IsEmpty())
            {
                throw new ArgumentException("param must have values", $"{nameof(keys)}");
            }
            var redisKeys = keys.ToRedisKeyCollection();
            return await database.KeyDeleteAsync(redisKeys.ToArray());
        }


    }
}
