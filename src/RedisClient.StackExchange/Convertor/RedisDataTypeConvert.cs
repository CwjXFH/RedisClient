using RedisClient.Models.Enums;
using StackExchange.Redis;

namespace RedisClient.StackExchange.Convertor
{
    internal static class RedisDataTypeConvert
    {
        public static RedisDataType ToRedisDataType(this RedisType redisType) => redisType switch
        {
            RedisType.None => RedisDataType.None,
            RedisType.String => RedisDataType.String,
            RedisType.List => RedisDataType.List,
            RedisType.Set => RedisDataType.Set,
            RedisType.SortedSet => RedisDataType.ZSet,
            RedisType.Hash => RedisDataType.Hash,
            RedisType.Stream => RedisDataType.Stream,
            _ => throw new NotSupportedException("Not supported type")
        };
    }
}
