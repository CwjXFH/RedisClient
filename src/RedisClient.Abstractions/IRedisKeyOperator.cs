using RedisClient.Models.Enums;
using RedisClient.Models.RedisResults;
using RedisClient.Models.RedisResults.Key;

namespace RedisClient.Abstractions
{
    /// <summary>
    /// Operate redis key
    /// </summary>
    /// <seealso cref="https://redis.io/commands#generic"/>
    public interface IRedisKeyOperator
    {
        #region Write Operation
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
        /// Like the Del command, it removes the specified key and ignored if a key does not exist.
        /// </summary>
        /// <remarks>
        /// The command performs the actual memory reclaiming in a different thread,
        /// so it is not blocking, while DEL is,
        /// </remarks>
        /// <returns>True if the key was removed.</returns>
        Task<bool> UnlinkAsync(string key, CancellationToken cancellationToken = default);
        /// <summary>
        /// Like the Del command, it removes the specified keys and ignored if a key does not exist.
        /// </summary>
        /// <remarks>
        /// The command performs the actual memory reclaiming in a different thread,
        /// so it is not blocking, while DEL is,
        /// </remarks>
        /// <returns>The number of keys that were unlinked.</returns>
        Task<long> UnlinkAsync(ICollection<string> keys, CancellationToken cancellationToken = default);
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
        /// Set an absolute unix timestamp on key. After that the key will deleted immediately.
        /// </summary>
        /// <param name="timestamp">An absolute <see href="https://en.wikipedia.org/wiki/Unix_time">unix timestamp</see> (seconds since January 1, 1970).</param>
        /// <param name="expireBehavior">Redis version >= 7.0.0: Added options: `NX`, `XX`, `GT` and `LT`</param>
        /// <returns>
        /// True if the timeout was set. Otherwise false. e.g. key doesn't exist, 
        /// or operation skipped due to the provided arguments.
        /// </returns>
        Task<bool> ExpireAtAsync(string key, long timestamp, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default);
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
        /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted.
        /// </summary>
        /// <param name="expireBehavior">Redis version >= 7.0.0: Added options: `NX`, `XX`, `GT` and `LT`</param>
        /// <returns>
        /// True if the timeout was set. Otherwise false. e.g. key doesn't exist, 
        /// or operation skipped due to the provided arguments.
        /// </returns>
        Task<bool> ExpireAsync(string key, TimeSpan timeSpan, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default);
        /// <summary>
        /// Set an absolute unix timestamp on key. After that the key will deleted immediately.
        /// </summary>
        /// <param name="expireBehavior">Redis version >= 7.0.0: Added options: `NX`, `XX`, `GT` and `LT`</param>
        /// <returns>
        /// True if the timeout was set. Otherwise false. e.g. key doesn't exist, 
        /// or operation skipped due to the provided arguments.
        /// </returns>
        Task<bool> ExpireAtAsync(string key, DateTime dateTime, KeySetExpireBehavior expireBehavior = KeySetExpireBehavior.None, CancellationToken cancellationToken = default);
        /// <summary>
        /// Alters the last access time of key. A key is ignored if it does not exist.
        /// </summary>
        Task<bool> TouchAsync(string key, CancellationToken cancellationToken = default);
        /// <summary>
        /// Alerts the last access time of keys. A key is ignored if it does not exist.
        /// </summary>
        /// <returns>The number of keys that were touched.</returns>
        Task<long> TouchAsync(ICollection<string> keys, CancellationToken cancellationToken = default);
        /// <summary>
        /// Remove the existing timeout on the key, that the key will never expire.
        /// </summary>
        /// <returns>True if the timeout was removed. Otherwise false. e.g. key doesn't exist, or key has no associated expire. </returns>
        Task<bool> PersistAsync(string key, CancellationToken cancellationToken = default);
        #endregion

        #region Read Operation
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
        /// <summary>
        /// Returns the absolute Unix timestamp in seconds at which the given key will expire.
        /// </summary>
        /// <returns>
        /// Expiration Unix timestamp in seconds, or
        /// -1 if the key exists but has no associated expiration time,
        /// -2 if the key does not exist.
        /// </returns>
        Task<OperationResult<ExpireTimeResult>> ExpireTimeAsync(string key, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns the absolute Unix timestamp in Milliseconds at which the given key will expire.
        /// </summary>
        /// <returns>
        /// Expiration Unix timestamp in Milliseconds, or
        /// -1 if the key exists but has no associated expiration time,
        /// -2 if the key does not exist.
        /// </returns>
        Task<OperationResult<ExpireTimeResult>> PExpireTimeAsync(string key, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns the type of the value stored at key.
        /// </summary>
        /// <returns>Type of key, or none when key does not exist.</returns>
        Task<RedisDataType> TypeAsync(string key, CancellationToken cancellationToken = default);
        #endregion
    }
}
