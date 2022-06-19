namespace RedisClient.Commons.Extensions
{
    public static class DateTimeExtension
    {
        private static readonly DateTime UnixTimestampStartTime =
            DateTime.Parse("1970-01-01T00:00:00.000Z").ToUniversalTime();

        public static long UnixSecondsTimestamp(this DateTime datetime)
            => (long)datetime.ToUniversalTime().Subtract(UnixTimestampStartTime).TotalSeconds;

        public static long UnixMillisecondsTimestamp(this DateTime datetime)
            => (long)datetime.ToUniversalTime().Subtract(UnixTimestampStartTime).TotalMilliseconds;
    }
}