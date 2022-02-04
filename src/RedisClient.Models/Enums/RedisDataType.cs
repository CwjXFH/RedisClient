namespace RedisClient.Models.Enums
{
    /// <summary>
    /// Type of the value stored at key.
    /// </summary>
    public enum RedisDataType
    {
        /// <summary>
        /// Key does not exist
        /// </summary>
        None = 0,
        String = 1,
        List = 2,
        Set = 3,
        ZSet = 4,
        Hash = 5,
        Stream = 6
    }
}
