using RedisClient.Commons.Extensions;
using RedisClient.Models.Exceptions;

namespace RedisClient.StackExchange.Internal
{
    internal static class ThrowHelper
    {
        /// <summary>
        /// Throw <see cref="RedisKeyInvalidException"/> if key is null or empty.
        /// </summary>
        /// <exception cref="RedisKeyInvalidException"></exception>
        internal static void ThrowIfKeyInvalid(string key) => ThrowIfExistInvalidKey(new[] { key });

        /// <summary>
        /// Throw <see cref="RedisKeyInvalidException"/> if key is null or empty.
        /// </summary>
        /// <exception cref="RedisKeyInvalidException"></exception>
        internal static void ThrowIfExistInvalidKey(IEnumerable<string> keyCollection)
        {
            if (keyCollection.IsEmpty())
            {
                throw new RedisKeyInvalidException("invalid key, key collection is empty");
            }

            foreach (var key in keyCollection)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new RedisKeyInvalidException("invalid key");
                }
            }
        }
    }
}
