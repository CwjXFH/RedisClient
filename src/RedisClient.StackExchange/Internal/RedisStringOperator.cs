﻿using RedisClient.Abstractions;
using RedisClient.Commons.Extensions;
using RedisClient.Commons.Lua;
using RedisClient.Models.Consts;
using RedisClient.Models.Enums;
using RedisClient.Models.Exceptions;
using RedisClient.Models.Extensions;
using RedisClient.Models.RedisResults;
using RedisClient.StackExchange.Convertor;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisClient.StackExchange.Internal
{
    internal class RedisStringOperator : RedisOperator, IRedisStringOperator
    {
        /// <summary>
        /// 2^29 - 1
        /// </summary>
        private const uint MaxSetRangeOffset = (2 << 28) - 1;

        public RedisStringOperator(IDatabase database)
            : base(database) { }

        #region Write Operation
        public async Task<long> AppendAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.StringAppendAsync(key, value);
        }

        public async Task<long> IncrByAsync(string key, long increment = 1, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.StringIncrementAsync(key, increment);
        }

        public async Task<long> DecrByAsync(string key, long increment = 1, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.StringDecrementAsync(key, increment);
        }

        public async Task<double> IncrByFloatAsync(string key, double increment, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.StringIncrementAsync(key, increment);
        }

        public async Task<long> SetRangeAsync(string key, uint offset, string value, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            if (offset > MaxSetRangeOffset)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), offset, $"The maximum allowed offset is {MaxSetRangeOffset}");
            }
            var result = await Database.StringSetRangeAsync(key, offset, value);
            return (long)result;
        }

        public async Task MSetAsync(IDictionary<string, string> keyValues, CancellationToken cancellationToken = default)
            => await MSetCoreAsync(keyValues, When.Always, cancellationToken);

        public async Task<bool> MSetNXAsync(IDictionary<string, string> keyValues, CancellationToken cancellationToken = default)
            => await MSetCoreAsync(keyValues, When.NotExists, cancellationToken);

        private async Task<bool> MSetCoreAsync(IDictionary<string, string> keyValues, When when, CancellationToken cancellationToken = default)
        {
            if (keyValues.IsEmpty())
            {
                throw new ArgumentException("Param must have values", $"{nameof(keyValues)}");
            }

            var kvPairs = keyValues.ToRedisKeyValueDict();
            return await Database.StringSetAsync(kvPairs.ToArray(), when);
        }
        #endregion

        #region Read Operation
        public async Task<string> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.StringGetAsync(key);
        }

        public async Task<T> GetAsync<T>(string key, T defaultValue, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            var redisVal = await GetAsync(key, cancellationToken);
            return JsonSerializer.Deserialize<T>(redisVal) ?? defaultValue;
        }

        public async Task<string> GetRangeAsync(string key, int start, int end, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.StringGetRangeAsync(key, start, end);
        }

        public async Task<OperationResult<IDictionary<string, string>>> MGetAsync(ICollection<string> keys, CancellationToken cancellationToken = default)
        {
            if (keys == null || keys.Count <= 0)
            {
                throw new ArgumentException("Param must have values", $"{nameof(keys)}");
            }

            var redisKeys = keys.ToRedisKeyCollection();
            var redisVal = await Database.StringGetAsync(redisKeys.ToArray());
            var result = new OperationResult<IDictionary<string, string>>(true, new Dictionary<string, string>());
            if (redisVal != null && redisVal.Length > 0)
            {
                for (var i = 0; i < redisKeys.Count; i++)
                {
                    var key = redisKeys[i];
                    var val = redisVal[i];
                    if (val.HasValue)
                    {
                        result.Data[key] = val.ToString();
                    }
                    else
                    {
                        result.Data[key] = "";
                    }
                }
            }
            return result;
        }

        public async Task<long> StrLenAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.StringLengthAsync(key);
        }
        #endregion

        #region Write & Read Operation
        public async Task<OperationResult<string>> SetAsync(string key, string value, TimeSpan? expiry = null, bool keepttl = false, KeyWriteBehavior writeBehavior = KeyWriteBehavior.None
            , bool returnOldValue = false, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            if (returnOldValue && writeBehavior == KeyWriteBehavior.NotExists)
            {
                throw new RedisCommandSyntaxErrorException("Syntax error");
            }

            var expiryArg = "";
            double expiryVal = -1;
            if (keepttl)
            {
                expiryArg = "KEEPTTL";
            }
            else if (expiry.HasValue)
            {
                expiryArg = "PX";
                expiryVal = expiry.Value.TotalMilliseconds;
            }
            var writeBehave = writeBehavior.StringValue();
            var get = returnOldValue ? "GET" : "";
            var parameters = new { key = (RedisKey)key, value, expiryArg, expiryVal, writeBehave, get };

            var luaScript = await GetLuaScriptAsync(LuaScriptName.StringOperatorScript.SET);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, parameters);

            /*
             Simple string reply: OK if SET was executed correctly.
             Null reply: (nil) if the SET operation was not performed because the user specified the NX or XX option but the condition was not met.
             If the command is issued with the GET option, the above does not apply. It will instead reply as follows, regardless if the SET was actually performed:
             Bulk string reply: the old string value stored at key.
             Null reply: (nil) if the key did not exist.
             */
            if (returnOldValue)
            {
                if (redisVal.Type == ResultType.BulkString)
                {
                    if (redisVal.IsNull && writeBehavior == KeyWriteBehavior.Exists)
                    {
                        return new OperationResult<string>(false, "");
                    }
                    return new OperationResult<string>(true, redisVal.IsNull ? "" : redisVal.ToString()!);
                }
            }
            else
            {
                if (redisVal.Type == ResultType.SimpleString && redisVal.IsNull == false
                    && string.Equals(redisVal.ToString(), RedisReturnValue.OK, StringComparison.OrdinalIgnoreCase))
                {
                    return new OperationResult<string>(true, "");
                }
                else if (redisVal.Type == ResultType.BulkString && redisVal.IsNull)
                {
                    return new OperationResult<string>(false, "");
                }
            }

            throw new RedisUnsupportedReturnValueException($"GET return type is {redisVal.Type}, value == null is {redisVal.IsNull}");
        }

        public async Task<OperationResult<T>> SetAsync<T>(string key, T value, TimeSpan? expiry, bool keepttl = false, KeyWriteBehavior writeBehavior = KeyWriteBehavior.None
            , bool returnOldValue = false, CancellationToken cancellationToken = default) where T : class
        {
            var redisVal = JsonSerializer.Serialize(value);
            var writeResult = await SetAsync(key, redisVal, expiry, keepttl, writeBehavior, returnOldValue, cancellationToken);
            return new OperationResult<T>(writeResult.Succeeded, default!);
        }

        public async Task<string> GetDelAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);
            return await Database.StringGetDeleteAsync(key);
        }

        public async Task<string> GetEXAsync(string key, TimeSpan? expiry, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfKeyInvalid(key);

            var expiryArg = "PERSIST";
            double expiryVal = -1;
            if (expiry.HasValue)
            {
                expiryArg = "PX";
                expiryVal = expiry.Value.TotalMilliseconds;
            }
            var parameters = new { key = (RedisKey)key, expiryArg, expiryVal };

            var luaScript = await GetLuaScriptAsync(LuaScriptName.StringOperatorScript.GETEX);
            var redisVal = await Database.ScriptEvaluateAsync(luaScript, parameters);

            if (redisVal == null || redisVal.IsNull)
            {
                return "";
            }
            else
            {
                return redisVal.ToString()!;
            }
        }
        #endregion
    }
}
