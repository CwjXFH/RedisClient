using RedisClient.Models.Enums;
using RedisClient.Models.RedisResults;
using RedisClient.Models.RedisResults.Key;

namespace RedisClient.Abstractions
{
    public interface IRedisKeyOperator
    {
        /// <summary>
        /// Removes the specified key. A key is ignored if it does not exist.
        /// </summary>
        /// <returns>True if the key was removed.</returns>
        Task<bool> DelAsync(string key, CancellationToken cancellationToken);
        /// <summary>
        /// Removes the specified keys. A key is ignored if it does not exist.
        /// </summary>
        /// <returns>The number of keys that were removed.</returns>
        Task<long> DelAsync(ICollection<string> keys, CancellationToken cancellationToken = default);
        /// <summary>
        /// Return if key exists.
        /// </summary>
        /// <returns>True if key exists.s</returns>
        Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
        /// <summary>
        /// Return if key exists.
        /// </summary>
        /// <remarks>Redis version >= 3.0.3: Accepts multiple `key` arguments.</remarks>
        /// <returns>Specifically the number of keys that exist from those specified as arguments.</returns>
        Task<long> ExistsAsync(ICollection<string> keys, CancellationToken cancellationToken = default);
        /// <summary>
        /// Set a timeout in seconds on key. After the timeout has expired, the key will automatically be deleted.
        /// </summary>
        /// <param name="seconds">The timeout to set.</param>
        /// <param name="expireBehavior">Redis version >= 7.0.0: Added options: `NX`, `XX`, `GT` and `LT`</param>
        /// <returns>
        /// True if the timeout was set. Otherwise false. e.g. key doesn't exist, 
        /// or operation skipped due to the provided arguments.
        /// </returns>
        Task<bool> ExpireAsync(string key, long seconds, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default);
        /// <summary>
        /// Set a timeout in milliseconds on key. After the timeout has expired, the key will automatically be deleted.
        /// </summary>
        /// <param name="milliseconds">The timeout to set.</param>
        /// <param name="expireBehavior">Redis version >= 7.0.0: Added options: `NX`, `XX`, `GT` and `LT`</param>
        /// <returns>
        /// True if the timeout was set. Otherwise false. e.g. key doesn't exist, 
        /// or operation skipped due to the provided arguments.
        /// </returns>
        Task<bool> PExpireAsync(string key, long milliseconds, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns the remaining time in seconds to live of a key that has a timeout.
        /// </summary>
        /// <returns>
        /// TTL in seconds, or
        /// -1 means key exists but has no associated expire,
        /// -2 key does not exist.
        /// </returns>
        Task<OperationResult<TTLResult>> TTLAsync(string key, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns the remaining time in milliseconds to live of a key that has a timeout.
        /// </summary>
        /// <returns>
        /// TTL in milliseconds, or
        /// -1 means key exists but has no associated expire,
        /// -2 key does not exist.
        /// </returns>
        Task<OperationResult<TTLResult>> PTTLAsync(string key, CancellationToken cancellationToken = default);
    }
}
