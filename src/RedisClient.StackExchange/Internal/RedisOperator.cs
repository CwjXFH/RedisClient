using StackExchange.Redis;

namespace RedisClient.StackExchange.Internal
{
    internal class RedisOperator
    {
        protected readonly IDatabase Database;

        public RedisOperator(IDatabase database)
        {
            this.Database = database;
        }
    }
}
