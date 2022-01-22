namespace RedisClient.Models.Consts
{
    public static class RedisParameter
    {
        public const string ExpireMilliseconds = "PX";
        public const string KeyExists = "XX";
        public const string KeyNotExists = "NX";
    }
}
