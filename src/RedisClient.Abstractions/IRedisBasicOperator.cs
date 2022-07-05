namespace RedisClient.Abstractions
{
    public interface IRedisBasicOperator
    {
        public IRedisKeyOperator KeyOperator { get; }
        public IRedisStringOperator StringOperator { get; }
        public IRedisHashOperator HashOperator { get; }
        public IRedisSetOperator SetOperator { get; }
        public IRedisSortedSetOperator SortedSetOperator { get; }
        public IRedisListOperator ListOperator { get; }
    }
}
