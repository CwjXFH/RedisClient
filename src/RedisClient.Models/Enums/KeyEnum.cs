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

    public enum KeyTTLType
    {
        KeyNotExists = -2,
        NoTTL = -1,
        HasTTL = 1
    }
}
