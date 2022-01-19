using Microsoft.Extensions.Options;
using RedisClient.Abstractions;
using RedisClient.StackExchange.Internal;
using RedisClient.Models.Options;
using StackExchange.Redis;

namespace RedisClient.StackExchange
{
    public class RedisBasicOperator : IRedisBasicOperator
    {
        private readonly IOptionsMonitor<RedisOptions> _optionsMonitor;
        private readonly IRedisConnectionFactory<ConnectionMultiplexer> _connectionFactory;

        private readonly IContravariantLazy<IRedisStringOperator> _stringOperator;
        private readonly IContravariantLazy<IRedisKeyOperator> _keyOperator;

        public RedisBasicOperator(IOptionsMonitor<RedisOptions> optionsMonitor, IRedisConnectionFactory<ConnectionMultiplexer> connectionFactory)
        {
            this._optionsMonitor = optionsMonitor;
            this._connectionFactory = connectionFactory;

            _stringOperator = CreateOperator<RedisStringOperator>();
            _keyOperator = CreateOperator<RedisKeyOperator>();
        }

        public IRedisKeyOperator KeyOperator => _keyOperator.Value;

        public IRedisStringOperator StringOperator => _stringOperator.Value;


        #region Lazy
        private IContravariantLazy<TOperator> CreateOperator<TOperator>() where TOperator : RedisOperator
            => new ContravariantLazy<TOperator>(() =>
            {
                var conn = _connectionFactory.Create();
                var db = conn.GetDatabase(_optionsMonitor.CurrentValue.DbIndex);
                return (Activator.CreateInstance(typeof(TOperator), db) as TOperator)!;
            }, true);

        private interface IContravariantLazy<out T>
        {
            T Value { get; }
        }

        private class ContravariantLazy<T> : Lazy<T>, IContravariantLazy<T>
        {
            public ContravariantLazy(Func<T> func, bool isThreadSafe)
                : base(func, isThreadSafe) { }

            public new T Value => base.Value;
        }
        #endregion
    }
}
