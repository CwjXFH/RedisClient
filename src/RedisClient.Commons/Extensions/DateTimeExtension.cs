namespace RedisClient.Commons.Extensions
{
    public static class DateTimeExtension
    {
        private static readonly DateTime UnixTimestampStartTime = new DateTime(1970, 1, 1);

        public static long UnixTimestamp(this DateTime datetime)
        {
            return (long)DateTime.UtcNow.Subtract(UnixTimestampStartTime).TotalSeconds;
        }

        public static long UnixMillisecondsTimestamp(this DateTime datetime)
        {
            return (long)DateTime.UtcNow.Subtract(UnixTimestampStartTime).TotalMilliseconds;
        }
    }
}
