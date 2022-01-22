using RedisClient.Abstractions;
using RedisClient.Commons.Extensions;
using RedisClient.Models.Enums;
using RedisClient.Models.Exceptions;
using RedisClient.Models.Extensions;
using RedisClient.Models.RedisResults;
using RedisClient.Models.RedisResults.Key;
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

        public async Task<bool> DelAsync(string key, CancellationToken cancellationToken)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await database.KeyDeleteAsync(key);
        }

        public async Task<long> DelAsync(ICollection<string> keys, CancellationToken cancellationToken = default)
        {
            if (keys.IsEmpty())
            {
                throw new ArgumentException("param must have values", $"{nameof(keys)}");
            }
            var redisKeys = keys.ToRedisKeyCollection();
            return await database.KeyDeleteAsync(redisKeys.ToArray());
        }

        public async Task<bool> ExpireAsync(string key, long seconds, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);

            var expireBehaviorVal = expireBehavior.StringValue();
            var parameters = new { key = (RedisKey)key, seconds, expireBehavior = expireBehaviorVal };

            var luaScript = LuaScript.Prepare(EXPIRELuaScript);
            var redisVal = await database.ScriptEvaluateAsync(luaScript, parameters);

            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                return (long)redisVal == 1;
            }

            throw new RedisUnsupportedReturnValueException($"EXPIRE return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<bool> PExpireAsync(string key, long milliseconds, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);

            var expireBehaviorVal = expireBehavior.StringValue();
            var parameters = new { key = (RedisKey)key, milliseconds, expireBehavior = expireBehaviorVal };

            var luaScript = LuaScript.Prepare(PEXPIRELuaScript);
            var redisVal = await database.ScriptEvaluateAsync(luaScript, parameters);

            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                return (long)redisVal == 1;
            }

            throw new RedisUnsupportedReturnValueException($"PEXPIRE return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<OperationResult<TTLResult>> TTLAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            var luaScript = LuaScript.Prepare(TTLLuaScript);
            var redisVal = await database.ScriptEvaluateAsync(luaScript, new { key });
            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                var val = (long)redisVal;
                var ttlResult = TTLResult.CreateTTLResult(val);

                return new OperationResult<TTLResult>(true, ttlResult);
            }

            throw new RedisUnsupportedReturnValueException($"TTL return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<OperationResult<TTLResult>> PTTLAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            var luaScript = LuaScript.Prepare(PTTLLuaScript);
            var redisVal = await database.ScriptEvaluateAsync(luaScript, new { key });
            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                var val = (long)redisVal;
                var ttlResult = TTLResult.CreateTTLResult(val);

                return new OperationResult<TTLResult>(true, ttlResult);
            }

            throw new RedisUnsupportedReturnValueException($"TTL return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }



        #region lua scripts
        private const string EXPIRELuaScript = @"local command = 'EXPIRE'
if @expireBehavior == '' then
    return redis.pcall(command, @key, @seconds)
else
    return redis.pcall(command, @key, @seconds, @expireBehavior)
end";
        private const string PEXPIRELuaScript = @"local command = 'PEXPIRE'
if @expireBehavior == '' then
    return redis.pcall(command, @key, @seconds)
else
    return redis.pcall(command, @key, @seconds, @expireBehavior)
end";

        private const string TTLLuaScript = @"return redis.pcall('TTL', @key)";
        private const string PTTLLuaScript = @"return redis.pcall('PTTL', @key)";
        #endregion

    }
}
