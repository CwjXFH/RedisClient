using RedisClient.Models.Enums;

namespace RedisClient.Models.RedisResults.Key
{
    public record ExpireTimeResult
    {
        private ExpireTimeResult(KeyExpireTimeResultType resultType, long timestamp)
        {
            ResultType = resultType;
            Timestamp = timestamp;
        }

        public KeyExpireTimeResultType ResultType { get; }

        public long Timestamp { get; }

        public static ExpireTimeResult CreateResult(long timestamp)
        {
            var resultType = KeyExpireTimeResultType.HasTimestamp;
            var timestampVal = 0L;

            if (timestamp == (int)KeyExpireTimeResultType.KeyNotExists)
            {
                resultType = KeyExpireTimeResultType.KeyNotExists;
            }
            else if (timestamp == (int)KeyExpireTimeResultType.NoTimestamp)
            {
                resultType = KeyExpireTimeResultType.NoTimestamp;
            }
            else if (timestamp >= 0)
            {
                timestampVal = timestamp;
            }
            else
            {
                throw new ArgumentException($"Invalid argument value: {timestamp}", $"{nameof(timestamp)}");
            }

            return new ExpireTimeResult(resultType, timestampVal);
        }
    }
}
