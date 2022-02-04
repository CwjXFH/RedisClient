namespace RedisClient.Models.Enums
{
    /// <summary>
    /// Indicates when this operation should be performed
    /// </summary>
    public enum KeyWriteBehavior
    {
        /// <summary>
        /// Default
        /// </summary>
        None = 0,
        /// <summary>
        /// The operation should only occur when there is an existing value 
        /// </summary>
        Exists = 1,
        /// <summary>
        /// The operation should only occur when there is not an existing value 
        /// </summary>
        NotExists = 2
    }

    /// <summary>
    /// Used for EXPIRE command
    /// </summary>
    public enum KeySetExpireBehavior
    {
        /// <summary>
        /// Default
        /// </summary>
        None = 0,
        /// <summary>
        /// Set expiry only when the key has no expiry
        /// </summary>
        Exists = 1,
        /// <summary>
        /// Set expiry only when the key has an existing expiry
        /// </summary>
        NotExists = 2,
        /// <summary>
        /// Set expiry only when the new expiry is greater than current one
        /// </summary>
        GreaterThan = 3,
        /// <summary>
        /// Set expiry only when the new expiry is less than the current one
        /// </summary>
        LessThan = 4
    }

    /// <summary>
    /// TTL command return type.
    /// </summary>
    public enum KeyTTLResultType
    {
        /// <summary>
        /// Key doesn't exist
        /// </summary>
        KeyNotExists = -2,
        /// <summary>
        /// Key exists, but has no associated expire
        /// </summary>
        NoTTL = -1,
        /// <summary>
        /// Key exists and associated expire
        /// </summary>
        HasTTL = 1
    }

    /// <summary>
    /// EXPIRETIME command return type.
    /// </summary>
    public enum KeyExpireTimeResultType
    {
        /// <summary>
        /// Key doesn't exist
        /// </summary>
        KeyNotExists = -2,
        /// <summary>
        /// Key exists, but has no associated expire
        /// </summary>
        NoTimestamp = -1,
        /// <summary>
        /// Key exists and associated expire
        /// </summary>
        HasTimestamp = 1
    }
}
