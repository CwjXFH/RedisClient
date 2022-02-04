using RedisClient.Models.Enums;

namespace RedisClient.Models.RedisResults.Key
{
    public record TTLResult
    {
        private TTLResult(KeyTTLResultType ttlType, long ttl)
        {
            ResultType = ttlType;
            TTL = ttl;
        }

        public KeyTTLResultType ResultType { get; }

        public long TTL { get; }

        public static TTLResult CreateTTLResult(long ttl)
        {
            var ttlType = KeyTTLResultType.HasTTL;
            var ttlVal = 0L;
            if (ttl == (int)KeyTTLResultType.NoTTL)
            {
                ttlType = KeyTTLResultType.NoTTL;
            }
            else if (ttl == (int)KeyTTLResultType.KeyNotExists)
            {
                ttlType = KeyTTLResultType.KeyNotExists;
            }
            else if (ttl >= 0)
            {
                ttlVal = ttl;
            }
            else
            {
                throw new ArgumentException($"Invalid argument value: {ttl}", $"{nameof(ttl)}");
            }

            return new TTLResult(ttlType, ttlVal);
        }
    }
}
