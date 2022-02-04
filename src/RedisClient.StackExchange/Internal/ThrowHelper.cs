using RedisClient.Models.Exceptions;

namespace RedisClient.StackExchange.Internal
{
    internal static class ThrowHelper
    {
        /// <summary>
        /// Throw <see cref="RedisKeyInvalidException"/> if key is null or empty.
        /// </summary>
        /// <exception cref="RedisKeyInvalidException"></exception>
        internal static void ThrowIfKeyInvalid(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new RedisKeyInvalidException("invalid key");
            }
        }
    }
}
