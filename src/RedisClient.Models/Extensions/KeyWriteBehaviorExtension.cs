using RedisClient.Models.Enums;

namespace RedisClient.Models.Extensions
{
    public static class KeyWriteBehaviorExtension
    {
        public static string StringValue(this KeyWriteBehavior writeBehavior)
        {
            var writeBehaviorVal = "";
            if (writeBehavior == KeyWriteBehavior.Exists)
            {
                writeBehaviorVal = "XX";
            }
            else if (writeBehavior == KeyWriteBehavior.NotExists)
            {
                writeBehaviorVal = "NX";
            }
            return writeBehaviorVal;
        }
    }
}
