using RedisClient.Abstractions;
using RedisClient.Commons.Extensions;
using RedisClient.Models.Enums;
using RedisClient.Models.Exceptions;
using RedisClient.Models.Extensions;
using RedisClient.Models.RedisResults;
using RedisClient.Models.RedisResults.Key;
using RedisClient.StackExchange.Convertor;
using RedisClient.StackExchange.Extensions;
using StackExchange.Redis;

namespace RedisClient.StackExchange.Internal
{
    internal class RedisKeyOperator : RedisOperator, IRedisKeyOperator
    {
        public RedisKeyOperator(IDatabase database)
            : base(database) { }

        #region Write Operation
        public async Task<bool> DelAsync(string key, CancellationToken cancellationToken)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.KeyDeleteAsync(key);
        }

        public async Task<long> DelAsync(ICollection<string> keys, CancellationToken cancellationToken = default)
        {
            if (keys.IsEmpty())
            {
                throw new ArgumentException("param must have values", $"{nameof(keys)}");
            }
            var redisKeys = keys.ToRedisKeyCollection();
            return await Database.KeyDeleteAsync(redisKeys.ToArray());
        }

        public async Task<bool> UnlinkAsync(string key, CancellationToken cancellationToken = default)
        {
            return await UnlinkAsync(new string[] { key }, cancellationToken) == 1;
        }

        public async Task<long> UnlinkAsync(ICollection<string> keys, CancellationToken cancellationToken = default)
        {
            var redisKeys = keys.ToRedisKeyCollection();

            var luaScript = LuaScript.Prepare(UNLINKLuaScript);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript.OriginalScript, redisKeys.ToArray(), new RedisValue[] { keys.Count });

            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                return (long)redisVal;
            }

            throw new RedisUnsupportedReturnValueException($"UNLINK return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<bool> ExpireAsync(string key, long seconds, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);

            var expireBehaviorVal = expireBehavior.StringValue();
            var parameters = new { key = (RedisKey)key, seconds, expireBehavior = expireBehaviorVal };

            var luaScript = LuaScript.Prepare(EXPIRELuaScript);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, parameters);

            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                return (long)redisVal == 1;
            }

            throw new RedisUnsupportedReturnValueException($"EXPIRE return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<bool> ExpireAtAsync(string key, long timestamp, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);

            var expireBehaviorVal = expireBehavior.StringValue();
            var parameters = new { key = (RedisKey)key, timestamp, expireBehavior = expireBehaviorVal };

            var luaScript = LuaScript.Prepare(EXPIREATLuaScript);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, parameters);

            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                return (long)redisVal == 1;
            }

            throw new RedisUnsupportedReturnValueException($"EXPIREAT return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<bool> PExpireAsync(string key, long milliseconds, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);

            var expireBehaviorVal = expireBehavior.StringValue();
            var parameters = new { key = (RedisKey)key, milliseconds, expireBehavior = expireBehaviorVal };

            var luaScript = LuaScript.Prepare(PEXPIRELuaScript);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, parameters);

            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                return (long)redisVal == 1;
            }

            throw new RedisUnsupportedReturnValueException($"PEXPIRE return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<bool> PExpireAtAsync(string key, long timestamp, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);

            var expireBehaviorVal = expireBehavior.StringValue();
            var parameters = new { key = (RedisKey)key, timestamp, expireBehavior = expireBehaviorVal };

            var luaScript = LuaScript.Prepare(PEXPIREATLuaScript);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, parameters);

            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                return (long)redisVal == 1;
            }

            throw new RedisUnsupportedReturnValueException($"PEXPIREAT return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<bool> ExpireAsync(string key, TimeSpan timeSpan, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default)
        {
            if (timeSpan.Milliseconds > 0)
            {
                return await PExpireAsync(key, (long)timeSpan.TotalMilliseconds, expireBehavior, cancellationToken);
            }

            return await ExpireAsync(key, (long)timeSpan.TotalSeconds, expireBehavior, cancellationToken);
        }

        public async Task<bool> ExpireAtAsync(string key, DateTime dateTime, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default)
        {
            if (dateTime.Millisecond > 0)
            {
                return await PExpireAtAsync(key, dateTime.UnixMillisecondsTimestamp(), expireBehavior, cancellationToken);
            }

            return await ExpireAtAsync(key, dateTime.UnixTimestamp(), expireBehavior, cancellationToken);
        }

        public async Task<bool> PersistAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.KeyPersistAsync(key);
        }
        #endregion

        #region Read Operation
        public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.KeyExistsAsync(key);
        }

        public async Task<long> ExistsAsync(ICollection<string> keys, CancellationToken cancellationToken = default)
        {
            var redisKeys = keys.ToRedisKeyCollection();
            return await Database.KeyExistsAsync(redisKeys.ToArray());
        }

        public async Task<OperationResult<TTLResult>> TTLAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            var luaScript = LuaScript.Prepare(TTLLuaScript);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, new { key });
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
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, new { key });
            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                var val = (long)redisVal;
                var ttlResult = TTLResult.CreateTTLResult(val);

                return new OperationResult<TTLResult>(true, ttlResult);
            }

            throw new RedisUnsupportedReturnValueException($"TTL return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<bool> TouchAsync(string key, CancellationToken cancellationToken = default)
        {
            return await TouchAsync(new[] { key }, cancellationToken) > 0;
        }

        public async Task<long> TouchAsync(ICollection<string> keys, CancellationToken cancellationToken = default)
        {
            var redisKeys = keys.ToRedisKeyCollection();
            return await Database.KeyTouchAsync(redisKeys.ToArray());
        }

        public async Task<OperationResult<ExpireTimeResult>> ExpireTimeAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            var luaScript = LuaScript.Prepare(EXPIRETIMELuaScript);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, new { key });
            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                var val = (long)redisVal;
                var result = ExpireTimeResult.CreateResult(val);

                return new OperationResult<ExpireTimeResult>(true, result);
            }

            throw new RedisUnsupportedReturnValueException($"EXPIRETIME return type is {redisVal.Type}, value == is {redisVal.IsNull}");
        }

        public async Task<OperationResult<ExpireTimeResult>> PExpireTimeAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            var luaScript = LuaScript.Prepare(PEXPIRETIMELuaScript);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, new { key });
            if (redisVal.Type == ResultType.Integer && redisVal.IsNull == false)
            {
                var val = (long)redisVal;
                var result = ExpireTimeResult.CreateResult(val);

                return new OperationResult<ExpireTimeResult>(true, result);
            }

            throw new RedisUnsupportedReturnValueException($"PEXPIRETIME return type is {redisVal.Type}, value == is {redisVal.IsNull}");
        }

        public async Task<RedisDataType> TypeAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            var redisVal = await Database.KeyTypeAsync(key);
            return redisVal.ToRedisDataType();
        }
        #endregion

        #region lua scripts

        private const string UNLINKLuaScript = @"local keyCount = ARGV[1]
local result = 0
local key = ''
for i = 1, keyCount, 1 do
    key = KEYS[i]
    result = redis.pcall('UNLINK', key) + result
end
return result
";

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
        private const string EXPIREATLuaScript = @"local command = 'EXPIREAT'
if @expireBehavior == '' then
    return redis.pcall(command, @key, @timestamp)
else
    return redis.pcall(command, @key, @timestamp, @expireBehavior)
end";
        private const string PEXPIREATLuaScript = @"local command = 'EXPIREAT'
if @expireBehavior == '' then
    return redis.pcall(command, @key, @timestamp)
else
    return redis.pcall(command, @key, @timestamp, @expireBehavior)
end";

        private const string TTLLuaScript = @"return redis.pcall('TTL', @key)";
        private const string PTTLLuaScript = @"return redis.pcall('PTTL', @key)";

        private const string EXPIRETIMELuaScript = @"return redis.pcall('EXPIRETIME', @key)";
        private const string PEXPIRETIMELuaScript = @"return redis.pcall('PEXPIRETIME', @key)";
        #endregion
    }
}
