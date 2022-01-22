using RedisClient.Models.Enums;

namespace RedisClient.Models.RedisResults.Key
{
    public record class TTLResult(KeyTTLType TTLType, long TTL)
    {
        public static TTLResult CreateTTLResult(long val)
        {
            var ttlType = KeyTTLType.HasTTL;
            var ttl = 0L;
            if (val == -1)
            {
                ttlType = KeyTTLType.NoTTL;
            }
            else if (val == -2)
            {
                ttlType = KeyTTLType.KeyNotExists;
            }
            else if (val >= 0)
            {
                ttl = val;
            }
            else
            {
                throw new ArgumentException($"Invalid argument value: {val}", $"{nameof(val)}");
            }

            return new TTLResult(ttlType, ttl);
        }
    }
}
