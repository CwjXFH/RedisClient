using RedisClient.Models.Enums;
using StackExchange.Redis;

namespace RedisClient.StackExchange.Convertor
{
    internal static class KeyWriteBehaviorConvert
    {
        /// <summary>
        /// Convert <see cref="KeyWriteBehavior"/> to <see cref="When"/>
        /// </summary>
        /// <param name="keyWriteBehavior"></param>
        /// <returns></returns>
        public static When ToWhen(this KeyWriteBehavior keyWriteBehavior) => keyWriteBehavior switch
        {
            KeyWriteBehavior.None => When.Always,
            KeyWriteBehavior.Exists => When.Exists,
            KeyWriteBehavior.NotExists => When.NotExists,
            _ => When.Always
        };
    }
}
