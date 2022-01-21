namespace RedisClient.Abstractions
{
    public interface IRedisKeyOperator
    {
        /// <summary>
        /// Removes the specified key. A key is ignored if it does not exist.
        /// </summary>
        /// <returns>True if the key was removed.</returns>
        Task<bool> DeleteAsync(string key, CancellationToken cancellationToken);
        /// <summary>
        /// Removes the specified keys. A key is ignored if it does not exist.
        /// </summary>
        /// <returns>The number of keys that were removed.</returns>
        Task<long> DeleteAsync(ICollection<string> keys, CancellationToken cancellationToken = default);
        /// <summary>
        /// Return if key exists.
        /// </summary>
        /// <returns>True if key exists.s</returns>
        Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
        /// <summary>
        /// Return if key exists.
        /// </summary>
        /// <returns>Specifically the number of keys that exist from those specified as arguments.</returns>
        Task<long> ExistsAsync(ICollection<string> keys, CancellationToken cancellationToken = default);

    }
}
