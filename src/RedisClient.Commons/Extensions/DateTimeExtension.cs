namespace RedisClient.Commons.Extensions
{
    public static class DateTimeExtension
    {
        public static long UnixTimestamp(this DateTime datetime)
        {
            return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static long UnixMillisecondsTimestamp(this DateTime datetime)
        {
            return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}
