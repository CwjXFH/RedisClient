using RedisClient.Models.Enums;

namespace RedisClient.Models.Extensions
{
    public static class KeySetExpireBehaviorExtension
    {
        public static string StringValue(this KeySetExpireBehavior expireBehavior)
        {
            var expireBehaviorVal = "";
            if (expireBehavior == KeySetExpireBehavior.Exists)
            {
                expireBehaviorVal = "XX";
            }
            else if (expireBehavior == KeySetExpireBehavior.NotExists)
            {
                expireBehaviorVal = "NX";
            }
            else if (expireBehavior == KeySetExpireBehavior.GreaterThan)
            {
                expireBehaviorVal = "GT";
            }
            else if (expireBehavior == KeySetExpireBehavior.LessThan)
            {
                expireBehaviorVal = "LT";
            }
            return expireBehaviorVal;
        }
    }
}
