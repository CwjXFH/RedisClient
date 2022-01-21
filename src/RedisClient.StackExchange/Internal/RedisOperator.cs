using StackExchange.Redis;

namespace RedisClient.StackExchange.Internal
{
    internal class RedisOperator
    {
        protected readonly IDatabase database;

        public RedisOperator(IDatabase database)
        {
            this.database = database;
        }
    }
}
