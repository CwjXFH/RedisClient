using RedisClient.Commons.Extensions;
using RedisClient.StackExchange.Internal;
using StackExchange.Redis;

namespace RedisClient.StackExchange.Extensions
{
    internal static class RedisKeyExtensions
    {
        /// <summary>
        /// Convert <see cref="RedisKey"/> collection to string collection.
        /// This method never return null.
        /// </summary>
        /// <exception cref="ArgumentException">Collection is null or has no element.</exception>
        /// <exception cref="RedisClient.Models.Exceptions.RedisKeyInvalidException">Key is null, empty, or only contains white-space characters.</exception>
        public static IList<string> ToStringCollection(this ICollection<RedisKey> redisKeys)
        {
            if (redisKeys.IsEmpty())
            {
                throw new ArgumentException("Collection must have elements", $"{nameof(redisKeys)}");
            }
            var result = new List<string>(redisKeys.Count);
            foreach (var key in redisKeys)
            {
                ThrowHelper.ThrowIfKeyInvalid(key);
                result.Add(key.ToString());
            }
            return result;
        }

        /// <summary>
        /// Convert string collection to <see cref="RedisKey"/> Collection.
        /// This method never return null.
        /// </summary>
        /// <exception cref="ArgumentException">Collection is null or has no element.</exception>
        /// <exception cref="RedisClient.Models.Exceptions.RedisKeyInvalidException">Key is null, empty, or only contains white-space characters.</exception>
        public static IList<RedisKey> ToRedisKeyCollection(this ICollection<string> stringKeys)
        {
            if (stringKeys.IsEmpty())
            {
                throw new ArgumentException("Collection must have elements", $"{nameof(stringKeys)}");
            }
            var result = new List<RedisKey>(stringKeys.Count);
            foreach (var key in stringKeys)
            {
                ThrowHelper.ThrowIfKeyInvalid(key);
                result.Add(key);
            }
            return result;
        }

        /// <summary>
        /// Convert <see cref="IDictionary{string, string}"/> to <see cref="IDictionary{RedisKey, RedisValue}"/>
        /// </summary>
        /// <param name="keyvalues"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="RedisClient.Models.Exceptions.RedisKeyInvalidException">Key is null, empty, or only contains white-space characters.</exception>
        public static IDictionary<RedisKey, RedisValue> ToRedisKeyValueDict(this IDictionary<string, string> keyvalues)
        {
            if (keyvalues.IsEmpty())
            {
                throw new ArgumentException("Collection must have elements", $"{nameof(keyvalues)}");
            }
            var result = new Dictionary<RedisKey, RedisValue>(keyvalues.Count);
            foreach (var kv in keyvalues)
            {
                ThrowHelper.ThrowIfKeyInvalid(kv.Key);
                result[kv.Key] = kv.Value;
            }
            return result;
        }

    }
}
