using RedisClient.Models.Exceptions;

namespace RedisClient.StackExchange.Internal
{
    internal static class ThrowHelper
    {
        internal static void ThrowIfKeyInvalid(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new RedisKeyInvalidException("invalid key");
            }
        }
    }
}
